using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField]
    public string Name;
    public float Weighting = 1f;
    List<Person> peopleWaiting = new List<Person>();
    public List<Person> GeneratePeople(int count)
    {
        List<Person> newPeople = new List<Person>();
        Debug.Log($"Generating {count} people at {Name} stop");
        for (int i = 0; i < count; i++)
        {
            Person person = new Person();
            person.CurrentStop = this;
            newPeople.Add(person);
        }
        peopleWaiting.AddRange(newPeople);
        return newPeople;
    }
}
