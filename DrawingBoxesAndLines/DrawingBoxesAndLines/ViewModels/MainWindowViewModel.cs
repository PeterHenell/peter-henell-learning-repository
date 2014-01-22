using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DrawingBoxesAndLines.ViewModels
{
    class Node : INotifyPropertyChanged
    {
        Node _parent;
        public Node Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }

        ObservableCollection<Node> _children;
        public ObservableCollection<Node> Children
        {
            get
            {
                return _children;
            }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    OnPropertyChanged("Children");
                }
            }
        }


        int _nodeId;
        public int NodeId
        {
            get
            {
                return _nodeId;
            }
            set
            {
                if (_nodeId != value)
                {
                    _nodeId = value;
                    OnPropertyChanged("NodeId");
                }
            }
        }


        int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }


        bool _hasChildren;
        public bool HasChildren
        {
            get
            {
                return _hasChildren;
            }
            set
            {
                if (_hasChildren != value)
                {
                    _hasChildren = value;
                    OnPropertyChanged("HasChildren");
                }
            }
        }

        public List<ExecutionItem> ExecutionItems { get; set; }

        private static int nodeCounter = 0;

        public Node AddChild(int repeatCount)
        {
            if (Children == null)
                Children = new ObservableCollection<Node>();

            var n = new Node(this.Level + 1, this, repeatCount);
            this.HasChildren = true;
            Children.Add(n);
            return n;
        }


        public static Node CreateLevelOneNode(int repeatCount)
        {
            return new Node(1, null, repeatCount);
        }

        public int RepeatCount { get; set; }

        private Node(int level, Node parent, int repeatCount)
        {
            Parent = parent;
            NodeId = nodeCounter++;
            ExecutionItems = new List<ExecutionItem>();
            RepeatCount = repeatCount;

            Level = level;
            HasChildren = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public Node RootNode { get; set; }

        Node _selectedNode;
        public Node SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                if (_selectedNode != value)
                {
                    _selectedNode = value;
                    OnPropertyChanged("SelectedNode");
                }
            }
        }

        public MainWindowViewModel()
        {
            RootNode = Node.CreateLevelOneNode(1);
            var player = RootNode.AddChild(100);
            player.AddExecutionItem("User");
            player.AddExecutionItem("UserDetails");
            player.AddExecutionItem("Account");
            player.AddExecutionItem("UserHistory");


            var playerSession = player.AddChild(1);
            playerSession.AddExecutionItem("UserSession");

            playerSession.AddChild(1).AddExecutionItem("Deposit into account").AddExecutionItem("MoneyTransaction").AddExecutionItem("MoneyTransfer_Log");

            var gameRound = playerSession.AddChild(50);
            gameRound.AddExecutionItem("Auction");
            gameRound.AddExecutionItem("AuctionTransaction");
            gameRound.AddExecutionItem("AuctionTransaction (reserve money)").AddExecutionItem("MoneyTransaction"); ;
            gameRound.AddExecutionItem("AuctionTransaction (withdraw)").AddExecutionItem("MoneyTransaction"); ;
            gameRound.AddExecutionItem("AuctionTransaction (deposit)").AddExecutionItem("MoneyTransaction"); ;
            gameRound.AddExecutionItem("Auction_Log");
            gameRound.AddExecutionItem("AcutionHouseContributionTransaction");

            playerSession.AddChild(1).AddExecutionItem("Withdraw all money from account").AddExecutionItem("MoneyTransaction");
            playerSession.AddChild(1).AddExecutionItem("Update UserSession END");

            
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }


    }
    static class Extensions
    {
        static Random random = new Random(37 * DateTime.Now.Millisecond);
        public static Node AddExecutionItem(this Node n, string tableName)
        {
            n.ExecutionItems.Add(new ExecutionItem(tableName));
            return n;
        }
    }
}
