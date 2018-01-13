﻿using Monkeyspeak;
using Monkeyspeak.Logging;

namespace Engine.Libraries
{
    /// <summary>
    /// Monkey Speak for Dream Info items,
    /// <para/>
    /// things ike Dream Name, Dream Owner, Dream URL ect
    /// </summary>
    public class MsDreamInfo : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 30;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            // Furre Enters
            // (0:30) When anyone enters the Dream,
            Add(TriggerCategory.Cause, 30,
                r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                "When anyone enters the Dream,");

            // (0:31) When the furre named {..} enters the Dream,
            Add(TriggerCategory.Cause, 31,
                r => NameIs(r),
                "When the furre named {..} enters the Dream,");

            // Furre Leaves
            // (0:32) When anyone leaves the Dream,
            Add(TriggerCategory.Cause, 32,
                r => ReadTriggeringFurreParams(r),
                "When anyone leaves the Dream,");

            // (0:33) When a furre named {..} leaves the Dream,
            Add(TriggerCategory.Cause, 33,
                r => NameIs(r),
                "When a furre named {..} leaves the Dream,");

            // (0:34) When the bot enters a Dream,
            Add(TriggerCategory.Cause, 34,
             r => ReadDreamParams(r),
                "When the bot enters a Dream,");

            // (0:35) When the bot enters the Dream named {..},
            Add(TriggerCategory.Cause, 35,
                r => DreamNameIs(r),
                "When the bot enters the Dream named {..},");

            // (0:36) When the bot leaves a Dream,
            Add(TriggerCategory.Cause, 36,
               r => ReadDreamParams(r),
               "When the bot leaves a Dream,");

            // (0:37) When the bot leaves the Dream named {..},
            Add(TriggerCategory.Cause, 37,
               r => DreamNameIs(r),
               "When the bot leaves the Dream named {..},");

            Add(TriggerCategory.Condition,
                 r => BotIsDreamOwner(r),
                 "and the bot is the Dream owner,");

            Add(TriggerCategory.Condition,
                r => !BotIsDreamOwner(r),
                " and the bot is not the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => DreamInfo.DreamOwner.ToFurcadiaShortName() == r.ReadString().ToLower(),
                " and the furre named {..} is the Dream owner,");

            Add(TriggerCategory.Condition,
                r => DreamInfo.DreamOwner.ToFurcadiaShortName() != r.ReadString().ToLower(),
                " and the furre named {..} is not the Dream owner,");

            Add(TriggerCategory.Condition,
              r => DreamNameIs(r),
              "and the Dream Name is {..},");

            Add(TriggerCategory.Condition,
                r => DreamNameIsNot(r),
                "and the Dream Name is not {..},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsDreamOwner(r),
                "and the triggering furre is the Dream owner,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsNotDreamOwner(r),
                "and the triggering furre is not the Dream owner,");

            Add(TriggerCategory.Condition,
                r =>
                (ParentBotSession.HasShare || DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName),
                 "and the bot has share control of the Dream or is the Dream owner,");

            Add(TriggerCategory.Condition,
                (r) => ParentBotSession.HasShare,
             "and the bot has share control of the Dream,");

            Add(TriggerCategory.Condition,
                (r) => !ParentBotSession.HasShare,
             "and the bot doesn't have share control in the Dream,");

            Add(TriggerCategory.Effect,
                r => ShareTrigFurre(r),
                "give share control to the triggering furre.");

            Add(TriggerCategory.Effect,
               r => UnshareTrigFurre(r),
               "remove share control from the triggering furre.");

            Add(TriggerCategory.Effect,
               r => UnshareFurreNamed(r),
               "remove share from the furre named {..} if they're in the Dream right now.");

            Add(TriggerCategory.Effect,
              r => ShareFurreNamed(r),
              "give share to the furre named {..} if they're in the Dream right now.");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private bool BotIsDreamOwner(TriggerReader reader)
        {
            return DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName;
        }

        private bool DreamNameIs(TriggerReader reader)
        {
            ReadDreamParams(reader);
            return DreamInfo.Name.ToLower() == reader.ReadString().ToLower();
        }

        private bool DreamNameIsNot(TriggerReader reader)
        {
            return !DreamNameIs(reader);
        }

        private bool ShareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                return SendServer($"share {Target.ShortName}");
            }

            Logger.Info($"{Target.Name} Is Not in the dream");
            return false;
        }

        private bool ShareTrigFurre(TriggerReader reader)
        {
            return SendServer($"share { Player.ShortName}");
        }

        private bool TriggeringFurreIsDreamOwner(TriggerReader reader)
        {
            return Player.ShortName == DreamInfo.DreamOwner.ToFurcadiaShortName();
        }

        private bool TriggeringFurreIsNotDreamOwner(TriggerReader reader)
        {
            return !TriggeringFurreIsDreamOwner(reader);
        }

        private bool UnshareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                return SendServer($"unshare {Target.ShortName}");
            }

            Logger.Info($"{Target.Name} Is Not in the dream");
            return false;
        }

        private bool UnshareTrigFurre(TriggerReader reader)
        {
            return SendServer($"unshare {Player.ShortName}");
        }

        #endregion Private Methods
    }
}