

create Procedure SolvePuzzle(@puzzle char(81))
as
begin
 
set nocount on
-- Set-based solution for Suduko puzzles.
-- Best part?  The only variables used are for keep tracking of the # of passes need and row counts.
-- Plus, we use normalized tables.  A true SQL solution, not
-- a procedural solution written in SQL.  :)
 
-- Our Main working table:
create table #Squares (Row int, Col int, Value int, primary key (Row,Col))
 
-- We keep track of values that are eliminated:
create table #Eliminated (Row int, Col int, Value int, primary key (Row,Col,Value))
 
-- The best thing you can use to solve almost any SQL problem, a "numbers" table:
create Table #Numbers (n int primary key)
 
insert into #Numbers
select 1 union all
select 2 union all
select 3 union all
select 4 union all
select 5 union all
select 6 union all
select 7 union all
select 8 union all
select 9
 
-- The key to our set-based solution:  a Groups table that
-- stores all sets of rows/cols into sets of GroupID's that
-- indicate which row/cols are grouped together.
-- (probably a bad explaination)
 
create table #Groups
  (GroupID int, r int, c int, primary key (GroupID,r,c))
 
  -- put in all horizontal lines in groups 1-9:
  insert into #Groups (GroupID, r, c)
  select R.N, R.N, C.N
  from #Numbers R
  cross join #Numbers C
 
  -- next, all vertical lines in groups 10-19:
 
  insert into #Groups (GroupID, r, c)
  select C.N + 9, R.N, C.N
  from #Numbers R
  cross join #Numbers C
 
  -- and all “blocks” in groups 20-29
  insert into #Groups (GroupID, r, c)
  select ((R.N-1)/3)*3 + (C.N-1)/3 + 19, R.N, C.N
  from #Numbers R
  cross join #Numbers C
 
-- Let's put starting the values into our Squares table:
insert into #Squares (Col,Row, Value)
select Col, Row, Val
from
            (select C.N as Col, R.N as Row, substring(@puzzle, (r.N-1) * 9 + c.N ,1) as Val
            from #Numbers R
            cross join #Numbers C
            ) x
where Val <>' ' and Val <>'0'
 
-- even though this is SQL, we must do a loop.
 
declare @Count integer;
declare @Pass integer;
 
set @pass = 0;
set @count = -1;

-- Here is where the work is done; it is simply 4 INSERT statements repeated until none of them
-- add any more rows.  At that point, we are done and either have solved it, or need to give up.
 
while (@count <> 0)
begin
            set @pass = @pass + 1
 
            -- Step 1: Eliminations Part I
            -- anything that is already entered on a square eliminates all other possibilities
            -- for that square:
 
            insert into #Eliminated (row, Col, value)
            select S.Row, S.Col, Vals.N
            From #Squares S
            cross join #Numbers Vals
            LEFT OUTER JOIN
              #Eliminated E
            on
              E.Row = S.Row and E.Col = S.Col and Vals.N = E.Value
            where
              E.Row is null
 
            set @count = @@rowcount
            
            -- Step 2:  Elminations part II
            -- Using our #groups table, which groups together all rows, columns and 'squares', we can 
            -- eliminate values for each group based on what is currently there:
            insert into #Eliminated (Row, Col, Value)
            select Others.R, Others.C, #Squares.value
            from
              #Squares
            inner join
              #Groups on #Groups.r = #Squares.row and #Groups.c = #Squares.col
            inner join
              #Groups Others on  #Groups.GroupID = Others.GroupID
            left outer join
              #Eliminated on #Eliminated.Row = Others.r and #Eliminated.Col = Others.C 
                          and #Eliminated.Value = #Squares.Value
            where
              #Eliminated.Value is null
            group by Others.R, Others.C, #Squares.Value
            
            set @count = @count + @@rowcount
 
            -- Step 3: Add Squares Part I
            -- Add numbers where there are 8 value eliminated for that row/col. 
            -- We add the one number *not* eliminated, of course:
            insert into #Squares (row,col,Value)
            select OneLeft.row, OneLeft.col, Vals.N
            from
                        ( select row, col 
                          from #Eliminated
                          group by row,col
                          having count(*)=8
                        ) OneLeft
            cross join
              #Numbers Vals
            left outer join
              #Eliminated E
            on
              OneLeft.Row = E.Row and OneLeft.Col = E.Col and Vals.N = E.Value
            where
              E.Value is null
 
            set @count = @count + @@rowcount
 
 
            -- Step 4:  Add Squares Part II
            -- If a number has been eliminated in all squares in a group except for 1,
            -- Add that number to the one square it has not been eliminated.
            -- We only execute this if the previous code could not add any new squares to avoid
            -- the need to update the #Elminated table again.
 
            if (@count = 0)
                        insert into #Squares (row, col, value)
                        select distinct G.r as Row, G.c as Col, GroupElims.Value
                        from
                                    (select groupID, Value
                                    from #Eliminated E
                                    inner join #Groups G
                                    on E.row = G.r and E.Col = G.c
                                    group by GroupID, Value
                                    having count(*) = 8
                                    ) GroupElims                              
                        inner join #Groups G 
                                    on G.GroupID = GroupElims.GroupID
                        left outer join #Eliminated E
                                    on E.row = G.r and E.col = g.c and E.Value = GroupElims.Value
                        where
                                    E.Value is null
 
            set @count = @count + @@rowcount        
 
            -- that's it. loop until all the steps add no rows.
end
 
set nocount off
 
if ((select count(*) from #Squares) <> 81) 
            -- we have failed.
            begin
                        print 'puzzle could not be solved! ... tried ' + convert(varchar(10), @Pass) + ' passes.'
            end
else
            begin
            -- we *think* we have succeeded ! 
 
            -- But we need to check.  because this is SQL and we have our #Groups table, the check is quite easy:
 
            if exists(select GroupID, Value
                        from #Squares S
                        inner join #Groups G
                        on S.row = G.r and S.col = G.c
                        group by GroupID, Value
                        having count(*) != 1)   -- If this SELECT returns any rows, a single group has a number
                                                          -- repeated more than once.
 
                        print ' Error! the solution is not valid'
            else
                        print 'Success in ' + convert(varchar(10), @Pass) + ' passes.'
            end
 
-- show final results:
-- (cross-tabbing the result to make it more readable)
select row, sum(case when col=1 then value else 0 end) as Col1,
            sum(case when col=2 then value else 0 end) as Col2,
            sum(case when col=3 then value else 0 end) as Col3,
            sum(case when col=4 then value else 0 end) as Col4,
            sum(case when col=5 then value else 0 end) as Col5,
            sum(case when col=6 then value else 0 end) as Col6,
            sum(case when col=7 then value else 0 end) as Col7,
            sum(case when col=8 then value else 0 end) as Col8,
            sum(case when col=9 then value else 0 end) as Col9
from #Squares
group by row
order by row
            
 
-- clean it up:
drop table #Squares
drop table #Eliminated
drop table #Groups
drop table #Numbers

end

EXEC SolvePuzzle ' 2 6 8 58 97 4 37 5 6 4 8 13 2 98 36 3 6 9 ' 