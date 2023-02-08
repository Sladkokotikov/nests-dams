public interface IConfirmationPerformer
{
    public ConfirmationAbility Confirm();
    public ConfirmationAbility ConfirmAuto();
    public ConfirmationAbility ConfirmRandom();
}

public interface IActionPerformer
{
    public ActionAbility Build();
    public ActionAbility Break();
    public ActionAbility Spawn();
    public ActionAbility Kill();
    public ActionAbility Push();
    public ActionAbility Pull();
    public ActionAbility Convert();
    public ActionAbility Invert();
    public ActionAbility Lock();
    public ActionAbility Unlock();
    public ActionAbility Draw();
    public ActionAbility Discard();
}

public interface ICriteriaPerformer
{
    public CriteriaAbility Magpie();
    public CriteriaAbility Beaver();
    public CriteriaAbility Obstacle();
    public CriteriaAbility Playable();
    public CriteriaAbility Adjacent();
    public CriteriaAbility Surrounding();
    public CriteriaAbility Plus();
    public CriteriaAbility Edge();
    public CriteriaAbility Free();
    public CriteriaAbility Occupied();
}