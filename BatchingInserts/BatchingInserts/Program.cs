using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.ComponentModel;
using System.Data.SqlClient;

namespace BatchingInserts
{


    class Program
    {
        public interface IProducerScenario
        {
            void RunScenario(ConcurrentQueue<FunData> queue);

            void Stop();
        }

        public class ProducePerDelayScenario : IProducerScenario
        {
            private readonly Random random;
            private bool runMore;
            private readonly int delayMs;
            private readonly int producedValuesPerDelay;

            public ProducePerDelayScenario(int delayMs, int producedValuesPerDelay)
            {
                int seed = (int)(DateTime.Now.Ticks % int.MaxValue);
                random = new Random(seed);
                runMore = true;
                this.delayMs = delayMs;
                this.producedValuesPerDelay = producedValuesPerDelay;
            }

            public void RunScenario(ConcurrentQueue<FunData> queue)
            {
                while (runMore)
                {
                    for (int i = 0; i < producedValuesPerDelay; i++)
                    {
                        queue.Enqueue(new FunData("some random data to fill in, can be big", Thread.CurrentThread.ManagedThreadId, random.Next()));
                    }

                    Thread.Sleep(delayMs);
                }
            }

            public override string ToString()
            {
                return string.Format("ProducePerDelayScenario Delay:{0}, Produces per delay: {1}", delayMs, producedValuesPerDelay);
            }

            public void Stop()
            {
                runMore = false;
            }
        }

        public struct FunData
        {
            public readonly string Data;
            public readonly int Owner;
            public readonly int Target;

            public FunData(string data, int owner, int target)
            {
                this.Data = data;
                this.Owner = owner;
                this.Target = target;
            }
        }

        public abstract class AbstractConsumer : IDisposable
        {
            protected SqlConnection connection;
            protected SqlCommand command;

            private readonly string insertQuery;

            private bool consumeMore;
            protected long consumeCount = 0;

            public AbstractConsumer(string insertQuery)
            {
                connection = new System.Data.SqlClient.SqlConnection("Data Source=localhost;Initial Catalog=PeterTrunk;Integrated Security=True");
                connection.Open();

                string tableCreation = @"
                IF exists(select 1 from INFORMATION_SCHEMA.tables WHERE TABLE_NAME = 'SomeMassiveTable')
	                DROP TABLE SomeMassiveTable;

                CREATE TABLE SomeMassiveTable(
	                SomeMassiveTableId BIGINT IDENTITY(1, 1) PRIMARY KEY CLUSTERED,
	                CreatedDate DATETIME2(7) NOT NULL DEFAULT(GETDATE()),
	                Data VARCHAR(MAX) NOT NULL,
	                OwnerId INT NOT NULL,
	                TargetId INT NOT NULL 
                )";

                using (command = new SqlCommand(tableCreation, connection))
                {
                    command.ExecuteNonQuery();
                }
                
                this.insertQuery = insertQuery;
                command = new SqlCommand(insertQuery, connection);
                consumeMore = true;
            }

            public void BeginConsume(ConcurrentQueue<FunData> items)
            {
                Action a = new Action(() =>
                {
                    using (command)
                    {
                        while (consumeMore)
                        {
                            FunData item;
                            if (items.TryDequeue(out item))
                            {
                                ImplementedConsume(item);
                            }
                        }
                    }
                });
                a.BeginInvoke(null, null);
            }

            protected abstract void ImplementedConsume(FunData item);

            public void Stop()
            {
                consumeMore = false;
            }

            public long GetComsumeCount()
            {
                return consumeCount;
            }

            public void Dispose()
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public class OneAtATimeConsumer : AbstractConsumer
        {
            public OneAtATimeConsumer()
                : base("INSERT dbo.SomeMassiveTable (Data,OwnerId,TargetId) VALUES  (@data, @ownerId, @targetId);")
            {
            }

            protected override void ImplementedConsume(FunData item)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@data", item.Data);
                command.Parameters.AddWithValue("@ownerId", item.Owner);
                command.Parameters.AddWithValue("@targetId", item.Target);
                command.ExecuteNonQuery();
                consumeCount++;
            }

            public override string ToString()
            {
                return "One Row At a time Consumer";
            }
        }

        public class BatchedInsertConsumer : AbstractConsumer
        {
            private static string insertQuery = @"INSERT dbo.SomeMassiveTable (Data,OwnerId,TargetId) 
VALUES 
(@data0, @ownerId0, @targetId0),
(@data1, @ownerId1, @targetId1),
(@data2, @ownerId2, @targetId2),
(@data3, @ownerId3, @targetId3),
(@data4, @ownerId4, @targetId4),
(@data5, @ownerId5, @targetId5),
(@data6, @ownerId6, @targetId6),
(@data7, @ownerId7, @targetId7),
(@data8, @ownerId8, @targetId8),
(@data9, @ownerId9, @targetId9);";

            public BatchedInsertConsumer()
                : base(insertQuery)
            {
            }

            protected override void ImplementedConsume(FunData item)
            {
                if (counter == 0)
                {
                    command.Parameters.Clear();
                }

                command.Parameters.AddWithValue("@data" + counter, item.Data);
                command.Parameters.AddWithValue("@ownerId" + counter, item.Owner);
                command.Parameters.AddWithValue("@targetId" + counter, item.Target);

                counter++;

                if (counter == 10)
                {
                    counter = 0;
                    command.ExecuteNonQuery();
                    consumeCount += 10;
                }
            }

            private int counter = 0;

            public override string ToString()
            {
                return "Batched Insert Consumer";
            }
        }

        public class Producer
        {
            public void BeginProduce(ConcurrentQueue<FunData> queue, IProducerScenario scenario, int numberOfActors)
            {
                Action a = new Action(() =>
                {
                    for (int i = 0; i < numberOfActors; i++)
                    {
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            scenario.RunScenario(queue);
                        }));
                        
                        t.Start();
                    }
                });
                a.BeginInvoke(null, null);
            }
        }


        public class Manager
        {
            public void RunFor(int runTime, IProducerScenario scenario, AbstractConsumer consumer)
            {
                Console.WriteLine();
                ConcurrentQueue<FunData> queue = new ConcurrentQueue<FunData>();
                Producer producer = new Producer();

                producer.BeginProduce(queue, scenario, 100);
                consumer.BeginConsume(queue);

                Thread.Sleep(runTime);
                consumer.Stop();
                scenario.Stop();

                Console.WriteLine("Results!");
                Console.WriteLine("Scenario: {0}", scenario.ToString());
                Console.WriteLine("Consumer: {0}", consumer.ToString());
                Console.WriteLine();
                Console.WriteLine("Left in queue: {0}", queue.Count);
                Console.WriteLine("Consumed: {0}", consumer.GetComsumeCount());
            }
        }

        static void Main(string[] args)
        {
            var runTime = 10000;

            Manager manager = new Manager();
            
            manager.RunFor(runTime, new ProducePerDelayScenario(1000, 5), new OneAtATimeConsumer());

            manager.RunFor(runTime, new ProducePerDelayScenario(1000, 5), new BatchedInsertConsumer());

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
