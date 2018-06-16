/*
 * This interface is used for all the game objects that should be registered somewhere 
 * (eg. in this case GameRegisterEntity.cs) in order to be easily founded in the 
 * other scripts.
 **/
public interface IRegistrable {
    /* With this method you can get the unique key of the gameobject. The networkId, 
     * since it should be unique, is used as key by Dictionaries in GameEntityRegister.cs
     * in order to store/find quickly and safe gameobjects.
     * **/
    uint GetUniqueKey();
}
