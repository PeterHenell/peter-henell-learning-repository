using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProducerNewDataStructurePOC
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    interface IProducer
    {
        DataResultSet Produce(IConsumer consumer, Container container);
    }


    interface IConsumer
    {
        //Action<string> IdentityCreatedCallback();
        IdentityValues Consume(DataResultSet data);

    }
    
    class Controler
    {
         public void Flow(IProducer producer, IConsumer consumer)
         {
             
         }
    }

    class Container
    {
        public Container(Container parent)
        {
            Parent = parent;
            Tables = new List<Table>();
            if (parent == null)
            {
                Level = 1;
            }
        }

        public List<Table> Tables { get; set; }
        public int ChildRepeatCount { get; set; }
        public Container Parent { get; set; }
        public List<Container> Children { get; set; }
        public int Level { get; set; }

        public void AddChild(Container child)
        {
            child.Level = this.Level + 1;
            Children.Add(child);
        }
    }

    class DataResultSet
    {
         
    }

    class IdentityValues
    { }

    class Table
    {
        public string TableName { get; set; }
        public Dictionary<String, Column> Columns { get; set; }
    }

    class Column
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }

    class Row
    {
         
    }

}

