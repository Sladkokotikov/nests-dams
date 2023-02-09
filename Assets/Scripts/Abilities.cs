using System.Collections.Generic;

public class ActionAbility : ICriteriaPerformer, IConfirmationPerformer
{
    private readonly List<byte> _bytecode;

    public ActionAbility(List<byte> bytecode = null)
    {
        _bytecode = bytecode ?? new List<byte>();
    }

    public CriteriaAbility Magpie() => new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Magpie)));

    public CriteriaAbility Beaver() => new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Beaver)));

    //public CriteriaAbility Obstacle()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Obstacle)));

    //public CriteriaAbility Playable()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Playable)));

    public CriteriaAbility Adjacent()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Adjacent)));

    public CriteriaAbility Surrounding()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Surrounding)));

    public CriteriaAbility Plus()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Plus)));

    public CriteriaAbility Edge()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Edge)));

    public CriteriaAbility Free()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Free)));

    public CriteriaAbility Occupied()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Occupied)));

    
    public ConfirmationAbility Confirm() => new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Confirm)));

    public ConfirmationAbility ConfirmAuto()=> new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.ConfirmAuto)));

    public ConfirmationAbility ConfirmRandom()=> new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.ConfirmRandom)));
}

public class CriteriaAbility : ICriteriaPerformer, IConfirmationPerformer
{
    private readonly List<byte> _bytecode;

    public CriteriaAbility(List<byte> bytecode = null)
    {
        _bytecode = bytecode ?? new List<byte>();
    }

    public CriteriaAbility Magpie() => new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Magpie)));

    public CriteriaAbility Beaver() => new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Beaver)));

    //public CriteriaAbility Obstacle()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Obstacle)));

    //public CriteriaAbility Playable()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Playable)));

    public CriteriaAbility Adjacent()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Adjacent)));

    public CriteriaAbility Surrounding()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Surrounding)));

    public CriteriaAbility Plus()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Plus)));

    public CriteriaAbility Edge()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Edge)));

    public CriteriaAbility Free()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Free)));

    public CriteriaAbility Occupied()=> new CriteriaAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Occupied)));
    
    public ConfirmationAbility Confirm() => new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Confirm)));

    public ConfirmationAbility ConfirmAuto()=> new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.ConfirmAuto)));

    public ConfirmationAbility ConfirmRandom()=> new ConfirmationAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.ConfirmRandom)));

}

public class ConfirmationAbility : IActionPerformer
{
    private readonly List<byte> _bytecode;

    public ConfirmationAbility(List<byte> bytecode = null)
    {
        _bytecode = bytecode ?? new List<byte>();
    }
    
    public IEnumerable<byte> Bytecode => _bytecode;

    public ActionAbility Build()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Build)));

    public ActionAbility Break()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Break)));

    public ActionAbility Spawn()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Spawn)));

    public ActionAbility Kill()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Kill)));

    public ActionAbility Push()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Push)));

    public ActionAbility Pull()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Pull)));

    public ActionAbility Lock()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Lock)));

    public ActionAbility Unlock()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Unlock)));

    public ActionAbility Draw()=> new ActionAbility(_bytecode.With(list => list.Add((byte) BytecodeBasis.Draw)));
}