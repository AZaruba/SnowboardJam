public class EntityData
{
    public uint u_entityID { get; set; }

    public EntityData()
    {
        this.u_entityID = GameMasterController.GetNextAvailableID();
    }
}
