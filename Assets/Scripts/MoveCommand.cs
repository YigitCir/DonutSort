using DG.Tweening;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Donut donut;
    private Pole fromPole;
    private Pole toPole;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private float moveDuration;

    public MoveCommand(Donut donut, Pole fromPole, Pole toPole, Vector3 fromPosition, Vector3 toPosition, float moveDuration)
    {
        this.donut = donut;
        this.fromPole = fromPole;
        this.toPole = toPole;
        this.fromPosition = fromPosition;
        this.toPosition = toPosition;
        this.moveDuration = moveDuration;
    }

    public void Execute()
    {
        
    }

    public void Undo()
    {
        toPole.RemoveDonut(donut); // Donut'ı hedef pole'dan çıkar
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(donut.transform.DOMove(donut.transform.position + Vector3.up * 2 , moveDuration))
            .Append(donut.transform.DOMoveY(fromPole.stackPosition.position.y + (fromPole.GetDonutCount() * fromPole.donutHeight), moveDuration))
            .Append(donut.transform.DOMove(fromPosition, moveDuration).OnComplete(() =>
        {
            fromPole.StackDonut(donut);
        }));
    }
}