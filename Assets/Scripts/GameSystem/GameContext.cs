public class GameContext
{
    public GameVarWrapper Owner = new NullVarWrapper();
    public GameVarWrapper Caster = new NullVarWrapper();
    public GameVarWrapper Target = new NullVarWrapper();

    public GameContext()
    {
        Owner = new NullVarWrapper();
        Caster = new NullVarWrapper();
        Target = new NullVarWrapper();
    }

    public GameContext(GameVarWrapper owner, GameVarWrapper caster, GameVarWrapper target)
    {
        Owner = owner;
        Caster = caster;
        Target = target;
    }
}
