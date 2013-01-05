using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.SSO;
using DAF.Social.Models;

namespace DAF.Social
{
    public interface ISocialProvider
    {
        Person GetPerson(string personId, bool loadAllInfo = false);
        bool SavePerson(Person person);
        bool RemovePerson(string personId);

        IEnumerable<PersonLink> GetPersonLinks(string personId, PersonLinkType[] linkTypes = null);
        IEnumerable<string> GetPersonLinkIds(string personId, PersonLinkType[] linkTypes = null);
        bool SavePersonLink(PersonLink personLink);
        bool RemovePersonLink(string personId, string linkPersonId);
    }
}
