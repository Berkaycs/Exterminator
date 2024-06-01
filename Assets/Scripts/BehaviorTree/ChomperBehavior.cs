using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperBehavior : BehaviorTree
{
    protected override void ConstructTree(out Node rootNode)
    {
        Selector rootSelector = new Selector();
        Sequencer attackTargetSequencer = new Sequencer();
        MoveToTargetTask moveToTarget = new MoveToTargetTask(this, "Target", 2f);
        attackTargetSequencer.AddChild(moveToTarget);

        BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(this, 
                                                        attackTargetSequencer, "Target", 
                                                        BlackboardDecorator.RunCondition.KeyNotExists, 
                                                        BlackboardDecorator.NotifyRule.RunConditionChange, 
                                                        BlackboardDecorator.NotifyAbort.Both);

        rootSelector.AddChild(attackTargetDecorator);


        Sequencer patrollingSequencer = new Sequencer();

        GetNextPatrolPointTask getNextPatrolPoint = new GetNextPatrolPointTask(this, "PatrolPoint");
        MoveToLocTask moveToPatrolPoint = new MoveToLocTask(this, "PatrolPoint", 3);
        WaitTask waitAtPatrolPoint = new WaitTask(2f);

        patrollingSequencer.AddChild(getNextPatrolPoint);
        patrollingSequencer.AddChild(moveToPatrolPoint);
        patrollingSequencer.AddChild(waitAtPatrolPoint);
        rootSelector.AddChild(patrollingSequencer);

        rootNode = rootSelector;
    }
}
