using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Social;
using DAF.Social.Models;

namespace DAF.Social.LocalProvider
{
    public class RepoSocialProvider : ISocialProvider
    {
        private ITransactionManager trans;
        private IIdGenerator generator;
        private IRepository<Org> repoOrg;
        private IRepository<Person> repoPerson;
        private IRepository<School> repoSchool;
        private IRepository<Contact> repoContact;
        private IRepository<PersonCertificate> repoPersonCert;
        private IRepository<PersonContact> repoPersonContact;
        private IRepository<PersonLink> repoPersonLink;
        private IRepository<StudyResume> repoStudyResume;
        private IRepository<WorkResume> repoWorkResume;
        
        public RepoSocialProvider(ITransactionManager trans, IIdGenerator generator,
            IRepository<Org> repoOrg, IRepository<Person> repoPerson, IRepository<School> repoSchool,
            IRepository<Contact> repoContact, IRepository<PersonCertificate> repoPersonCert, IRepository<PersonContact> repoPersonContact,
            IRepository<PersonLink> repoPersonLink, IRepository<StudyResume> repoStudyResume, IRepository<WorkResume> repoWorkResume)
        {
            this.trans = trans;
            this.generator = generator;
            this.repoOrg = repoOrg;
            this.repoPerson = repoPerson;
            this.repoSchool = repoSchool;
            this.repoContact = repoContact;
            this.repoPersonCert = repoPersonCert;
            this.repoPersonContact = repoPersonContact;
            this.repoPersonLink = repoPersonLink;
            this.repoStudyResume = repoStudyResume;
            this.repoWorkResume = repoWorkResume;
        }

        public Person GetPerson(string personId, bool loadAllInfo = false)
        {
            var person = repoPerson.Query(o => o.PersonId == personId).FirstOrDefault();
            if (person != null && loadAllInfo)
            {
                person.Contacts = repoPersonContact.Query(o => o.PersonId == personId).ToArray();
                person.Certificates = repoPersonCert.Query(o => o.PersonId == personId).ToArray();
                person.WorkResume = repoWorkResume.Query(o => o.PersonId == personId).ToArray();
                person.StudyResume = repoStudyResume.Query(o => o.PersonId == personId).ToArray();
                person.WorkResume.ForEach(w =>
                    {
                        w.Org = repoOrg.Query(o => o.OrgId == w.OrgId).FirstOrDefault();
                    });
                person.StudyResume.ForEach(w =>
                {
                    w.School = repoSchool.Query(o => o.SchoolId == w.SchoolId).FirstOrDefault();
                });
            }

            return person;
        }

        public bool SavePerson(Person person)
        {
            if (string.IsNullOrEmpty(person.PersonId))
            {
                person.PersonId = generator.NewId();
            }
            return repoPerson.Save(o => o.PersonId == person.PersonId, person);
        }

        public bool RemovePerson(string personId)
        {
            return repoPerson.DeleteBatch(o => o.PersonId == personId);
        }

        public IEnumerable<PersonLink> GetPersonLinks(string personId, PersonLinkType[] linkTypes = null)
        {
            var query = repoPersonLink.Query(o => o.PersonId == personId);
            if (linkTypes != null && linkTypes.Length > 0)
            {
                query = query.Where(o => linkTypes.Contains(o.LinkType));
            }
            return query.ToArray();
        }

        public IEnumerable<string> GetPersonLinkIds(string personId, PersonLinkType[] linkTypes = null)
        {
            var query = repoPersonLink.Query(o => o.PersonId == personId);
            if (linkTypes != null && linkTypes.Length > 0)
            {
                query = query.Where(o => linkTypes.Contains(o.LinkType));
            }
            var ids = query.Select(o => o.LinkPersonId).ToArray();
            return ids;
        }

        public bool SavePersonLink(PersonLink personLink)
        {
            return repoPersonLink.Save(o => o.PersonId == personLink.PersonId && o.LinkPersonId == personLink.LinkPersonId, personLink);
        }

        public bool RemovePersonLink(string personId, string linkPersonId)
        {
            return repoPersonLink.DeleteBatch(o => o.PersonId == personId && o.LinkPersonId == linkPersonId);
        }
    }
}
