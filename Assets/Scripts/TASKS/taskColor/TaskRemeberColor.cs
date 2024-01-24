using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskRemeberColor : MonoBehaviour
{
    int rnd;
    float timefloat=1f;
    [SerializeField] int HowManyColors;
    int level=0;
    List<int> RandomColors = new List<int>();
    List<int> SelectedColors= new List<int>();
    [SerializeField] GameObject[] Leds;
    [SerializeField] Collider[] Buttons;
    public GameObject player;
    public void ResetColor()
    {
        SelectedColors.Clear();
        RandomColors.Clear();
        for(int i=0;i<HowManyColors;i++)
        {
            rnd=Random.Range(0,4);
            RandomColors.Add(rnd);
        }
        //for(int i=0;i<RandomColors.Count;i++)Debug.Log(RandomColors[i]);
        StartCoroutine(ExampleCoroutine());
    }
    public void AddSelectedColor(int selected)
    {
        if(RandomColors.Count==HowManyColors)
        {
            SelectedColors.Add(selected);
            Debug.Log("selected "+selected);
            if(SelectedColors.Count==HowManyColors)
            {
                if(CheckColors())
                {
                    level++;
                    HowManyColors++;
                    Debug.Log("level "+level);
                }
                SelectedColors.Clear();
                RandomColors.Clear();
            }
            if(level==3)
            {
                player.GetComponent<JobHandler2>().VarTask=true;
            }
        }
        
    }
    bool CheckColors()
    {
        for(int i=0;i<SelectedColors.Count;i++)
            if(SelectedColors[i]!=RandomColors[i])
                return false;
                
        return true;
    }
    IEnumerator ExampleCoroutine()
    {
        for(int i=0;i<Buttons.Length;i++)
            Buttons[i].enabled=false;
        
        yield return new WaitForSeconds(timefloat);
        for(int i=0;i<RandomColors.Count;i++)
        {
            Leds[RandomColors[i]].SetActive(true);
            yield return new WaitForSeconds(timefloat);
            Leds[RandomColors[i]].SetActive(false);
            yield return new WaitForSeconds(timefloat);
        }
        for(int i=0;i<Buttons.Length;i++)
            Buttons[i].enabled=true;
    }
    
}
