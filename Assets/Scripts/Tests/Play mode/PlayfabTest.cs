using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayfabTest
{
    [UnityTest]
    public IEnumerator NetworkTest()
    {
        //Init
        SceneManager.LoadScene(0);
        yield return null;
        var UI = Manager.Instance.UIManager;
        var playfab = Manager.Instance.playfabLogin;
        var fusion = Manager.Instance.lobbyManager;

        //Playfab
        UI.SetMail("d@d.d");
        UI.SetPassword("123456");
        playfab.LoginButtonMethod();

        //Fusion
        while (!fusion.JoinedLobby)
        {
            yield return null;
            if (playfab.LoginError)
            {
                Assert.Fail("Playfab error.");
            }
        }
        Assert.IsTrue(fusion.JoinedLobby);
    }
}