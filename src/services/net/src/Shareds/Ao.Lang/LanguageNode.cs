using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace Ao.Lang
{
    internal class LanguageNode : ObservableCollection<ILanguageMetadata>, IList<ILanguageMetadata>, ICollection<ILanguageMetadata>, IEnumerable<ILanguageMetadata>, ICultureIdentity, ILanguageNode
    {
        public LanguageNode(CultureInfo culture)
        {
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            CollectionChanged += LanguageNode_CollectionChanged;
            ReBuild();
        }

        private void LanguageNode_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Move && ReBuildIfCollectionChanged)
            {
                ReBuild();
            }
        }

        private Lazy<ILanguageRoot> root;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ILanguageRoot Root => root.Value;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsBuilt => root.IsValueCreated;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CultureInfo Culture { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool ReBuildIfCollectionChanged { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ReBuild()
        {
            root = new Lazy<ILanguageRoot>(Build, true);
        }

        private ILanguageRoot Build()
        {
            var builder = new LanguageBuilder(Culture);
            var sources = this.SelectMany(s => s.LanguageSources).ToArray();
            foreach (var item in sources)
            {
                builder.Add(item);
            }
            return builder.Build();
        }
    }
}
