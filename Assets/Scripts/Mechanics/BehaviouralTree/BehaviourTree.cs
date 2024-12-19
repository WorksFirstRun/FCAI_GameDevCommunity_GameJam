using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviourTreeNamespace
{
    public class Node
    {
        public enum Status{Success,Fail,Running}

        public List<Node> childern = new List<Node>();
        protected int currentChild;
        protected Node activeLeafNode;
        protected Node parent;

        public void AddChild(Node child) => childern.Add(child);

        public virtual Status Evaluate() => childern[currentChild].Evaluate();

        public virtual void Reset()
        {
            currentChild = 0;
            foreach (Node child in childern)
            {
                child.Reset();
            }
        }

        public virtual Node StartNode()
        {
            return childern[currentChild].StartNode();
        }
        
        public void SetCurrentChild(int childIndex)
        {
            if (parent == null)
            {
                Reset();
                currentChild = childIndex;
                activeLeafNode = childern[currentChild].StartNode();
                return;
            }
            
            parent.SetCurrentChild(childIndex);
            
        }

        public Node GetActiveLeafNode()
        {
            if (parent == null)
            {
                return activeLeafNode;
            }

            return parent.GetActiveLeafNode();
        }

        protected void SetParentActiveLeafNode(Node n)
        {
            if (parent == null)
            {
                return;
            }
            parent.activeLeafNode = n;
            parent.SetParentActiveLeafNode(n);
        }
        
        protected void SwitchNode()
        {
            currentChild++;
            if (currentChild < childern.Count)
            {
                activeLeafNode = childern[currentChild].StartNode();
                if (parent != null)
                {
                    SetParentActiveLeafNode(activeLeafNode);
                }
            }
        }

        public void SetParent(Node parent)
        {
            this.parent = parent;
        }

    }


    public class BehaviourTree : Node
    {
        private bool firstRun;
        private bool isDisabled;
        
        public override Status Evaluate()
        {
            if (isDisabled) return Status.Fail;
            if (!firstRun)
            {
                activeLeafNode = base.StartNode();
                firstRun = true;
            }
            
            Status childEvaluateResult = childern[currentChild].Evaluate();
            currentChild = (currentChild + 1) % childern.Count;
            return childEvaluateResult;
        }

        public void PrintCurrentActiveNode()
        {
            Debug.Log(childern[currentChild].GetActiveLeafNode());
        }

        public void DisableTheTree()
        {
            base.Reset();
            isDisabled = true;
        }
    }


    public class Sequence : Node
    {
        public Sequence(Node parent)
        {
            this.parent = parent;
        }
        
        public override Status Evaluate()
        {
            if (currentChild < childern.Count)
            {
                switch (childern[currentChild].Evaluate())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Fail:
                        Reset();
                        return Status.Fail;
                    default:
                        SwitchNode();
                        return currentChild == childern.Count ? Status.Success : Status.Running;
                }
            }
            
            Reset();
            activeLeafNode = childern[currentChild].StartNode();
            if (parent != null)
            {
                SetParentActiveLeafNode(activeLeafNode);
            }
            return Status.Success;
        }
        
        
    }

    public class Selector : Node
    {

        public Selector(Node parent)
        {
            this.parent = parent;
        }
        
        public override Status Evaluate()
        {
            if (currentChild < childern.Count)
            {
                switch (childern[currentChild].Evaluate())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        activeLeafNode = childern[currentChild].StartNode();
                        if (parent != null)
                        {
                            SetParentActiveLeafNode(activeLeafNode);
                        }
                        return Status.Success;
                    default:
                        childern[currentChild].Reset();
                        SwitchNode();
                        return Status.Running;
                }
            }
            
            Reset();
            activeLeafNode = childern[currentChild].StartNode();
            if (parent != null)
            {
                SetParentActiveLeafNode(activeLeafNode);
            }
            return Status.Fail;
        }
        
        
    }
}
