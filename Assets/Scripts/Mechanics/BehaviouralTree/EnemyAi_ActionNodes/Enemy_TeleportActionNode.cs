using System;
using System.Collections.Generic;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_TeleportActionNode<Tteleport> : Node where Tteleport : Enum
    {
        private Context teleportContextRequirements;
        private Tteleport teleportStateName;
        private List<Transform> rooms;
        private Transform currentRoom;
        private float timer;
        private float maxNodeActiveTime;
        
        public Enemy_TeleportActionNode(Context teleportContextRequirements, Node parent,Transform currentRoom,
            List<Transform> rooms,BossHealth bossHealth,Tteleport teleportStateName)
        {
            this.teleportStateName = teleportStateName;
            this.parent = parent;
            this.teleportContextRequirements = teleportContextRequirements;
            maxNodeActiveTime =
                teleportContextRequirements.animation_VisualsHandler.GetAnimationClipTime(teleportStateName);
            this.currentRoom = currentRoom;
            this.rooms = rooms;
            bossHealth.OnBossHealthChanged += ActiveTeleportingNode;
        }


        private void ActiveTeleportingNode()
        {
            teleportContextRequirements.isTeleporting = true;
        }
        
        
        private void PickNewRoomAndTeleportTo()
        {
            rooms.Remove(currentRoom);
            Transform newRoom = rooms[Random.Range(0,rooms.Count)];
            rooms.Add(currentRoom);
            currentRoom = newRoom;
            teleportContextRequirements.entityTransform.position = currentRoom.position;
        }
        
        
        public override Node StartNode()
        {
            teleportContextRequirements.animation_VisualsHandler.SwitchAnimation(teleportStateName);
            return this;
        }

        public override Status Evaluate()
        {
            timer += Time.deltaTime;
            if (timer > maxNodeActiveTime)
            {
                PickNewRoomAndTeleportTo();
                return Status.Success;
            }
            return Status.Running;
        }

        public override void Reset()
        {
            timer = 0;
            teleportContextRequirements.isTeleporting = false;
        }
        
        
    }
}