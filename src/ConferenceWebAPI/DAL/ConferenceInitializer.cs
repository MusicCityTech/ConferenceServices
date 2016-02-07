using ConferenceWebAPI.Models;
using System.Collections.Generic;

namespace ConferenceWebAPI.DAL
{
    public class ConferenceInitializer : System.Data.Entity.DropCreateDatabaseAlways<ConferenceContext>
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

            context.Speakers.AddRange(speakers);
            context.SaveChanges();
        }
    }
}