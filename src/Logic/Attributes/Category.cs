using System;

namespace AlphaGuardian01.src.Logic.Attributes
{
    public enum CommandCategories
    {
        Moderator,
        Administrator,
        Fun,
        Miscellaneous
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CommandCategory : Attribute
    {
        private readonly CommandCategories _category;
        public CommandCategory(CommandCategories category)
        {
            _category = category;
        }
        public CommandCategories Category { get { return _category; } }
    }
}
