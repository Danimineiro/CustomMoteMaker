using Verse;

namespace Danis_Motes;
public record PawnPressedArguments
{
    public Pawn Pawn { get; }
    public bool Handled { get; set; }

    public PawnPressedArguments(Pawn pawn) => Pawn = pawn;
}

public delegate void PawnPressedEventHandler(PawnPressedArguments args);
