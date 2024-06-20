using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace Test
{
    public class Solution
    {
        private readonly IList<CheeseMongersItem> Items;

        public Solution(IList<CheeseMongersItem> items)
        {
            Items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                switch (item.Name)
                {
                    case "Ricotta":                    
                        CalculateQualityForCheese(item);
                        break;
                    case "Parmigiano Regiano":
                        item.Quality++;
                        break;
                    case "Caciocavallo Podolico":
                        item.Quality = item.Quality > 0 ? item.Quality : 0 ;
                        break;
                    case "Tasting with Chef Massimo":
                        CalculateTestingWithMassimo((MassimoEvent)item);
                        DecreaseEventDurarion((MassimoEvent)item);
                        break;
                }

                DecreaseValidityByDays(item);
            }
        }

        private void DecreaseEventDurarion(MassimoEvent massimoEventItem)
        {
            massimoEventItem.Duration = massimoEventItem.Duration > 0 ? massimoEventItem.Duration-- : 0;
        }

        private void CalculateTestingWithMassimo(MassimoEvent massimoEventItem)
        {
            var increase = 1;           

            if(massimoEventItem.Duration == 0)
            {
                massimoEventItem.Quality = 0;
            }
            else
            {
                if (massimoEventItem.Duration > 7 && massimoEventItem.Duration < 15)
                {
                    increase = increase + 3;
                }
                else if (massimoEventItem.Duration > -1 && massimoEventItem.Duration <= 7)
                {
                    increase = increase + 5;
                }

                massimoEventItem.Quality += increase;
            }

            if (massimoEventItem.Quality < 100)
            {
                massimoEventItem.Quality = massimoEventItem.Quality > 0 ? massimoEventItem.Quality + 1 : 0;
            }

            if (massimoEventItem.ValidByDays < 15 && massimoEventItem.Quality < 100)
            {
                QualityByValidityCalculation(massimoEventItem);
            }

            if (massimoEventItem.ValidByDays < 8 && massimoEventItem.Quality < 100)
            {
                QualityByValidityCalculation(massimoEventItem);
            }
                      
        }

        private void QualityByValidityCalculation(MassimoEvent massimoEventItem)
        {
            if (massimoEventItem.Quality + 2 <= 100)
            {
                massimoEventItem.Quality = massimoEventItem.Quality > 0 ? massimoEventItem.Quality + 2 : 0;
            }
            else
            {
                massimoEventItem.Quality = 100;
            }
        }

        private void CalculateQualityForCheese(CheeseMongersItem cheeseMongersItem)
        {
            int decreaseInQuality = 1;
            var difference = 0;  

            if (cheeseMongersItem.Quality < 100)
            {
                if (cheeseMongersItem.ValidByDays <= 0)
                {
                    decreaseInQuality = 5;
                }

                if (cheeseMongersItem.Name == CheeseTypesNames.Ricotta.ToString())
                {
                    difference = cheeseMongersItem.Quality - decreaseInQuality * 3;
                    cheeseMongersItem.Quality = difference > 0 ? difference : 0;
                }
                else
                {
                    difference = cheeseMongersItem.Quality - decreaseInQuality;
                    cheeseMongersItem.Quality = difference > 0 ? difference : 0;
                }
            }
        }

        private void DecreaseValidityByDays(CheeseMongersItem cheeseMongersItem)
        {
            if (cheeseMongersItem.Name != CheeseTypesNames.CaciocavalloPodolico.ToString())
            {
                cheeseMongersItem.ValidByDays = cheeseMongersItem.ValidByDays - 1;
            }
        }
    }

    enum CheeseTypesNames
    {
        [Description("Ricotta")]
        Ricotta,
        [Description("Parmigiano Regiano")]
        ParmigianoRegiano,
        [Description("Caciocavallo Podolico")]
        CaciocavalloPodolico
    }

    public class EventItem : CheeseMongersItem
    {
        public ItemType EventType { get; set; }
    }

    public class MassimoEvent : EventItem
    {
        public MassimoEvent()
        {
            EventType = ItemType.Tasting;
        }
        public int Duration { get; set; }
    }

    public enum ItemType
    {
        Tasting,
        Cheese
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
