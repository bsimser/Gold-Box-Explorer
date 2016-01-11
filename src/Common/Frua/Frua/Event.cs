using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    internal abstract class Event : IEventStrategy
    {
        protected Event()
        {
            Name = "Event";
        }

        public string Name { get; set; }

        #region IEventStrategy Members

        public virtual Event LoadEvent(BinaryReader reader)
        {
            var buffer = reader.ReadBytes(16);
            return this;
        }

        #endregion
    }

    internal class NullEvent : Event
    {
        public NullEvent()
        {
            Name = "No Event";
        }
    }

    internal class CombatEvent : Event
    {
        public CombatEvent()
        {
            Name = "Combat Event";
        }
    }

    internal class TextStatementEvent : Event
    {
        public TextStatementEvent()
        {
            Name = "Text Statement";
        }
    }

    internal class GiveTreasureEvent : Event
    {
        public GiveTreasureEvent()
        {
            Name = "Give Treasure";
        }
    }

    internal class DamageEvent : Event
    {
        public DamageEvent()
        {
            Name = "Damage";
        }
    }

    internal class StairsEvent : Event
    {
        public StairsEvent()
        {
            Name = "Stairs";
        }
    }

    internal class TrainingHallEvent : Event
    {
        public TrainingHallEvent()
        {
            Name = "Training Hall";
        }
    }

    internal class TavernEvent : Event
    {
        public TavernEvent()
        {
            Name = "Tavern";
        }
    }

    internal class ShopEvent : Event
    {
        public ShopEvent()
        {
            Name = "Shop";
        }
    }

    internal class TempleEvent : Event
    {
        public TempleEvent()
        {
            Name = "Temple";
        }
    }

    internal class QuestionButtonEvent : Event
    {
        public QuestionButtonEvent()
        {
            Name = "Question-Button";
        }
    }

    internal class TransferModuleEvent : Event
    {
        public TransferModuleEvent()
        {
            Name = "Transfer Module";
        }
    }

    internal class GuidedTourEvent : Event
    {
        public GuidedTourEvent()
        {
            Name = "Guided Tour";
        }
    }

    internal class AddNpcEvent : Event
    {
        public AddNpcEvent()
        {
            Name = "Add NPC";
        }
    }

    internal class NpcSaysEvent : Event
    {
        public NpcSaysEvent()
        {
            Name = "NPC Says";
        }
    }

    internal class EncounterEvent : Event
    {
        public EncounterEvent()
        {
            Name = "Encounter";
        }
    }

    internal class UtilityEvent : Event
    {
        public UtilityEvent()
        {
            Name = "Utilities";
        }
    }

    internal class SoundsEvent : Event
    {
        public SoundsEvent()
        {
            Name = "Sounds";
        }
    }

    internal class WhoTriesEvent : Event
    {
        public WhoTriesEvent()
        {
            Name = "Who Trys";
        }
    }

    internal class WhoPaysEvent : Event
    {
        public WhoPaysEvent()
        {
            Name = "Who Pays";
        }
    }

    internal class EnterPasswordEvent : Event
    {
        public EnterPasswordEvent()
        {
            Name = "Enter Password";
        }
    }

    internal class QuestionListEvent : Event
    {
        public QuestionListEvent()
        {
            Name = "Question-List";
        }
    }

    internal class SmallTownEvent : Event
    {
        public SmallTownEvent()
        {
            Name = "Small Town";
        }
    }

    internal class ChainEvent : Event
    {
        public ChainEvent()
        {
            Name = "Chain";
        }
    }

    internal class VaultEvent : Event
    {
        public VaultEvent()
        {
            Name = "Vault";
        }
    }

    internal class CombatTreasureEvent : Event
    {
        public CombatTreasureEvent()
        {
            Name = "Combat Treasure";
        }
    }

    internal class GainExperienceEvent : Event
    {
        public GainExperienceEvent()
        {
            Name = "Gain Experience";
        }
    }

    internal class PassTimeEvent : Event
    {
        public PassTimeEvent()
        {
            Name = "Pass Time";
        }
    }

    internal class CampEvent : Event
    {
        public CampEvent()
        {
            Name = "Camp";
        }
    }

    internal class RemoveNpcEvent : Event
    {
        public RemoveNpcEvent()
        {
            Name = "Remove NPC";
        }
    }

    internal class PickOneCombatEvent : Event
    {
        public PickOneCombatEvent()
        {
            Name = "Pick One Combat";
        }
    }

    internal class TeleporterEvent : Event
    {
        public TeleporterEvent()
        {
            Name = "Teleporter";
        }
    }

    internal class QuestStageEvent : Event
    {
        public QuestStageEvent()
        {
            Name = "Quest Stage";
        }
    }

    internal class QuestionYesNoEvent : Event
    {
        public QuestionYesNoEvent()
        {
            Name = "Question-Yes/No";
        }
    }

    internal class TavernTalesEvent : Event
    {
        public TavernTalesEvent()
        {
            Name = "Tavern Tales";
        }
    }

    internal class SpecialItemEvent : Event
    {
        public SpecialItemEvent()
        {
            Name = "Special Item";
        }
    }
}