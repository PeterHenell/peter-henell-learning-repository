//import java.*;
import java.util.*;


class Field
{
	private int size;
	private int startPos;
	private int[][] spelplan;
	
	private int agentX = 0; // läsas in från filen oxo!
	private int agentY = 0;
	
	public static int TREE = 84;
	public static int PLANT = 80;
	public static int EMPTY = 0;
	
	Field()
	{
		size = 5;
		startPos = 0;
		spelplan = new int[size][size];
		for (int i = 0; i < size; i++)
			for( int ii = 0; ii < size; ii++)
				spelplan[i][ii] = EMPTY; // TODO: Läs in spelplanen
		spelplan[1][0] = TREE;
		spelplan[1][1] = TREE;
		spelplan[1][2] = TREE;
		spelplan[1][4] = TREE;
		spelplan[3][2] = TREE;
		spelplan[3][3] = PLANT;
	}

	public int checkTile(int pos)
	{
                System.out.println("x: " +(pos%size) + " - y: " + (pos/size));
		return spelplan[pos % size][pos / size];
	}
	public int getStartPos()
	{
		return startPos;
	}
	
	public Node[] getChildren(Node parent)
	{
		Node[] children = new Node[4];
		try {
		
			if( checkTile(parent.getPos()) == TREE)
			{
				children[0] = null;
				children[1] = null;
				children[2] = null;
				children[3] = null;
				return children;
			}
			System.out.println(parent.getPos() - size);
			if (parent.getMove() != Node.SOUTH && !(parent.getPos() < size)){
				Node tmpNode = new Node(parent.getPos() - size, Node.NORTH);
                                tmpNode.setParent(parent);
				children[Node.NORTH] = tmpNode;
              			System.out.println("getChild NORTH!");
                        
			}
			else
				children[Node.NORTH] = null;
			
			if (parent.getMove() != Node.NORTH && !(parent.getPos() >= (size * size) - size)){
				Node tmpNode = new Node(parent.getPos() + size, Node.SOUTH);
                                tmpNode.setParent(parent);
				children[Node.SOUTH] = tmpNode;
              			System.out.println("getChild SOUTH!");
			}
			else
				children[Node.SOUTH] = null;
			
			if (parent.getMove() != Node.WEST && !(parent.getPos() % size == size - 1)){
				Node tmpNode = new Node(parent.getPos() + 1, Node.EAST);
                                tmpNode.setParent(parent);
				children[Node.EAST] = tmpNode;
              			System.out.println("getChild EAST!");                            
			}
			else
				children[Node.EAST] = null;

			if (parent.getMove() != Node.EAST && !(parent.getPos() % size == 0)){
				Node tmpNode = new Node(parent.getPos() - 1, Node.WEST);
                                tmpNode.setParent(parent);
				children[Node.WEST] = tmpNode;
              			System.out.println("getChild WEST!");
			}
			else
				children[Node.WEST] = null;

		}catch(Exception e) {System.out.println("Felkod 2234!");
		}
		
		return children;
	}
	 
}

class Node
{
	// En node är en riktining som vi har gått i, med mera
	public static int DEFAULT = -1;
	public static int NORTH = 2;
	public static int EAST = 0;
	public static int SOUTH = 1;
	public static int WEST = 3;
	private Node parent;
	private int move;
	private int pos;
		
	public Node(int apos, int amove)
	{
		this.move = amove;
		this.pos = apos;
	}
	public String translateMove()
	{
            if(move == NORTH)
		return "North";
            if(move == EAST)
		return "East";
            if(move == WEST)
		return "West";
            if(move == SOUTH)
		return "South";
            return "Default";
	}
        public Node getParent()
        {
            return parent;
        }
        public void setParent(Node Parent)
        {
            parent = Parent;
        }
	private void setPos(int apos)
	{
		this.pos = apos;
	}
	private void setMove(int amove)
	{
		this.move = amove;
	}
	public void setNode(int apos, int amove)
	{
		setPos(apos);
		setMove(amove);
	}
	public int getPos()
	{
		return this.pos;
	}
	public int getMove()
	{
            return move;
	}
}

public class Agent
{
	Field myWorld;
	private Vector OPEN;
	private Vector CLOSED;
	private Vector PATH;
        private Vector SOLUTION;
	private Node root;
	
	
	//private int size = 5;
	public static void main(String[] args)
	{
		Agent agent = new Agent();
		boolean done = agent.findFood();
                agent.printSolution(done);
	}
	public Agent()
	{
		myWorld = new Field();
		OPEN = new Vector();
		CLOSED = new Vector();
		PATH = new Vector();
                SOLUTION = new Vector();
	}
        public void printSolution(boolean done)
        {
            if(!done)
            {
                System.out.println("We got Fucked...;(");
                return;
            }
            System.out.println("Total moves: " +PATH.size()+ "\nCorrect moves: "+SOLUTION.size());
            for(int i = 0; i < SOLUTION.size(); i++)
            {
                if(i+1 < SOLUTION.size())
                    System.out.println("We are at: "+((Node)SOLUTION.elementAt(i)).getPos()+" and we intend to go "+((Node)SOLUTION.elementAt(i+1)).translateMove());
                else
                    System.out.println("We are at: "+((Node)SOLUTION.elementAt(i)).getPos()+" and we do not intend to go anywhere.");
            }
             
                
        }
	public boolean findFood()
	{
		//Create a node S from the start state
		//Create a list, OPEN := [S], containing all nodes not yet expanded
		//Create a set CLOSED := [], containing all nodes already expanded
		//loop
		//	if OPEN = [] then return failure
		//	else
		//N := first element in OPEN
		//	Remove N from OPEN
			//Add N to CLOSED
		//if N represents a goal state then
			//return SOLUTION
			//else
		//NODES := the set of nodes we get by applying all applicable
			//operators to N
		root = new Node(myWorld.getStartPos(), Node.DEFAULT); 
		Node[] tmpNodes = myWorld.getChildren(root);
		for (int i = 0; i < 4; i++)
		{
			if(tmpNodes[i] != null)
				OPEN.add(tmpNodes[i]);
		}
		 
		
		CLOSED.add(root);
		PATH.add(root);
		
		while(1==1)
		{
		
			if ( OPEN.size() == 0 ){
                                System.out.println("return failure "  + PATH.toString());
				return false;
                        }
			Node N = (Node)OPEN.remove(0);
			CLOSED.add(N);			
			PATH.add(N);
			System.out.println("Current Position: "  + N.getPos());
			if (myWorld.checkTile(N.getPos()) == Field.PLANT)
                        {
                                while(N.getPos() != root.getPos())
                                {
                                    SOLUTION.add(0, N);
                                    N = N.getParent();
                                }
                               SOLUTION.add(0, N);
                                System.out.println("Flower Found!!!! return PATH:" + PATH.toString());
				return true;
                        }
			tmpNodes = myWorld.getChildren(N);
			
			//Remove all nodes in NODES that are members of OPEN or CLOSED
			for (int i = 0; i < OPEN.size(); i++)
				for (int child = 0; child < 4; child++)
				{
					if ( tmpNodes[child] != null)
						if ( ((Node)OPEN.elementAt(i)).getPos() == tmpNodes[child].getPos() )
							tmpNodes[child] = null;
				}
			
			for (int i = 0; i < CLOSED.size(); i++)
				for (int child = 0; child < 4; child++)
				{
					if ( tmpNodes[child] != null)
						if ( ((Node)CLOSED.elementAt(i)).getPos() == tmpNodes[child].getPos() )
							tmpNodes[child] = null;
				}
			expandDepth(tmpNodes);
                        //expandBreadth(tmpNodes);
			System.out.println("OPEN is the shit:" + OPEN.toString());
		}
	}
	private void expandDepth(Node[] nodes)
	{
		for (int i = 3; i >= 0; i--)
		{
			if(nodes[i] != null)
                        {
				OPEN.add(0, nodes[i]);
                        }
		}
		
	}
	private void expandBreadth(Node[] nodes)
	{
		for (int i = 3; i >= 0; i--)
		{
			if(nodes[i] != null)
                        {
				OPEN.add(nodes[i]);
                        }
		}
		
	}
	
}
