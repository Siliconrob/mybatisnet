#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2008-06-28 09:26:16 -0600 (Sat, 28 Jun 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2008/2005 - Apache Fondation
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion


using System.Collections.Concurrent;
using System.Threading;

namespace MyBatis.DataMapper.Session.Stores
{
	/// <summary>
	/// Provides an implementation of <see cref="ISessionStore"/>
	/// which relies on <c>CallContext</c>.
    /// This implementation will first get the current session from the current 
    /// thread. Do NOT use on web scenario (web applications or web services).
	/// </summary>
	public class CallContextSessionStore : AbstractSessionStore
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="CallContextSessionStore"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public CallContextSessionStore(string id): base(id)
		{}

		/// <summary>
		/// Get the local session
		/// </summary>
        public override ISession CurrentSession
		{
            get { return (ISession)CallContext.GetData(sessionName); }
		}

		/// <summary>
		/// Store the specified session.
		/// </summary>
		/// <param name="session">The session to store</param>
        public override void Store(ISession session)
		{
			CallContext.SetData(sessionName, session);
		}

		/// <summary>
		/// Remove the local session.
		/// </summary>
		public override void Dispose()
		{
			CallContext.SetData(sessionName, null);
		}
	}

    public static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object data) =>
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object GetData(string name) =>
            state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }
}
