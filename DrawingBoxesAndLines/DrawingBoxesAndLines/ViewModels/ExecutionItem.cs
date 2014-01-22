using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DrawingBoxesAndLines.ViewModels
{
    class ExecutionItem
    {
        public string TargetTable { get; set; }
        
        public ExecutionItem(string tableName)
        {
            TargetTable = tableName;
        }
    }


    //class Manager : IDisposable
    //{
    //    IDataReader _reader;
    //    Comsumer _consumer;

    //    public Manager(IDataReader reader, Comsumer consumer)
    //    {
    //        _reader = reader;
    //        _consumer = consumer;
    //    }

    //    public void DoWork()
    //    {
    //        _consumer.Consume(_reader);
    //    }

    //    public void Dispose()
    //    {
            
    //    }
    //}

    //class Comsumer : IDisposable
    //{
    //    internal void Consume(System.Data.IDataReader _reader)
    //    {
            

    //    }
    //    public void Dispose()
    //    {
    //        //
    //    }
    //}

}
