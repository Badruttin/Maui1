using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MauiTest1
{
	public class VidRabot
	{
		public VidRabot()
		{
		}

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            VidRabot vidrabot = obj as VidRabot;
            return this.Id == vidrabot.Id;
        }

    }

}

