using ConferenceWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebAPI.DAL
{
    public class ConferenceInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConferenceContext>
    {
        protected override void Seed(ConferenceContext context)
        {
            var speakers = new List<Speaker>
            {
            new Speaker{FirstName="Carson",LastName="Alexander",
                Sessions=new List<Session> {new Session{Title="Chemistry"}}},
            new Speaker{FirstName="Meredith",LastName="Alonso",
                Sessions=new List<Session> {new Session{Title= "Microeconomics" } }},
            new Speaker{FirstName="Arturo",LastName="Anand",
                Sessions=new List<Session> {new Session{Title= "Macroeconomics" } }},
            new Speaker{FirstName="Gytis",LastName="Barzdukas",
                Sessions=new List<Session> {new Session{Title= "Calculus" } }},
            new Speaker{FirstName="Yan",LastName="Li",
                Sessions=new List<Session> {new Session{Title= "Trigonometry" } }},
            new Speaker{FirstName="Peggy",LastName="Justice",
                Sessions=new List<Session> {new Session{Title= "Composition" } }},
            new Speaker{FirstName="Laura",LastName="Norman",
                Sessions=new List<Session> {new Session{Title= "Literature" } }},
            new Speaker{FirstName="Nino",LastName="Olivetto",
                Sessions=new List<Session> {new Session{Title="Computer Science"}}}
            };

            speakers.ForEach(s => context.Speakers.Add(s));
            context.SaveChanges();
        }
    }
}