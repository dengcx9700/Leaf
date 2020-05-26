using Ao;
using Ao.Core;
using System;
using System.Collections.Generic;

namespace Ao.Project
{
    /// <summary>
    /// 表示一个工程部分
    /// </summary>
    public abstract class ProjectPart : NotifyableObject,IProjectPart
    {
        public static readonly string TypeAttributeName = "Type";
        private Project project;
        public ProjectPart()
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Project Project 
        {
            get => project;
            internal set 
            {
                RaisePropertyChanged(ref project, value);
                OnProjectChanged(value);
            }
        }

        /// <summary>
        /// 表示是否已经完成了
        /// </summary>
        public bool Done { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Reset()
        {
            var status = Done;
            OnReset(status);
            Done = false;
        }
        protected virtual void OnReset(bool status)
        {

        }
        protected virtual void OnProjectChanged(Project project)
        {

        }
    }
}
