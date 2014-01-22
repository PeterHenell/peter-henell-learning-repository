using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GenerateDataTable
{
    class Program
    {
        static void Main(string[] args)
        {


        class Node
        {
            public Node Parent { get; set; }
            public List<Node> Children { get; set; }
            public int NodeId { get; set; }

            public List<ExecutionItems> ExecutionItems { get; set; }

           
        }

        //private static DataTable CreateSet(TableEntity table)
        //{
        //    TableEntity dt = table.Clone();
        //    for (int i = 1; i < 100; i++)
        //    {
        //        DataRow row = dt.NewRow(i);
        //        dt.Rows.Add(row);
        //    }
        //    return dt;
        //}


        //   TableEntity table = new TableEntity("", "");
        //    long n = 1;
        //    var result = table.GenerateRows( () =>{ return n++;}, 100);

        //    foreach (DataRow row in result.Rows)
        //    {
        //        for (int i = 0; i < result.Columns.Count; i++)
        //        {
        //            Console.Write(row[i]);
        //            Console.Write(", ");
        //        }
        //        Console.WriteLine();
        //    }
        //    DataColumn
        //    DataRow

    

    public class TableEntity : DataTable
    {

        
        public string S { get; set; }
        public string  N { get; set; }

        public DataTable InnerTable { get; set; }

        public TableEntity(string n, string s)
        {
            S = s;
            N = n;

            InnerTable = new DataTable();
            Columns = new List<ColumnEntity>();

            foreach (var c in Enumerable.Range(1, 10))
            {
                Columns.Add(new ColumnEntity(c.ToString()));
                InnerTable.Columns.Add(new DataColumn(c.ToString(), typeof(object)));
            }
        }

        internal List<ColumnEntity> Columns { get; set; }

        public DataTable GenerateRows(Func<long> getN, int count)
        {
            DataTable clone = InnerTable.Clone();
            
            for (int k = 0; k < count; k++)
            {
                DataRow row = clone.NewRow();
                for (int i = 0; i < clone.Columns.Count; i++)
                {
                    row[i] = Columns[i].GenerateValue(getN());
                }
                clone.Rows.Add(row);
            }
            

            return clone;
        }
        }
    }

    class ColumnEntity : DataColumn
    {
        public string ColumnName { get; set; }
        

        public ColumnEntity (string columnName)
        {
            ColumnName = columnName;
        }

        public string GenerateValue(long n)
        {
            return n.ToString();
        }

        public object Generator { get; set; }
        public IList<object> PossibleGenerators { get; set; }
    }

    public class RowEntity : DataRow
    {
        public string Value { get; set; }

        public void GenerateValue(long n)
        {
            Value = n.ToString();
        }

        internal RowEntity(DataRowBuilder builder)
            : base(builder)
        {
        }
    }
}
