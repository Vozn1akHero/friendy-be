using System;
using System.Collections.Generic;
using System.Text;
using BE.Models;

namespace BE.FakeData.Builders
{
    public class FakeUserBuilder
    {
        private int _id;
        private string _name;
        private string _surname;
        private Gender _gender;
        private DateTime _birthday;
        private string _avatar;
        private Education _education;
        private Session _session;
        private UserAdditionalInfo _additionalInfo;

        public FakeUserBuilder(int id)
        {
            _id = id;
        }

        public FakeUserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public FakeUserBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }
        public FakeUserBuilder WithGender(Gender gender)
        {
            _gender = gender;
            return this;
        }
        public FakeUserBuilder WithBirthday(DateTime birthday)
        {
            _birthday = birthday;
            return this;
        }
        public FakeUserBuilder WithEducation(Education education)
        {
            _education = education;
            return this;
        }
        public FakeUserBuilder WithSession(Session session)
        {
           _session = session;
            return this;
        }
        public FakeUserBuilder WithAdditionalInfo(UserAdditionalInfo additionalInfo)
        {
            _additionalInfo = additionalInfo;
            return this;
        }

        public User Build()
        {
            return new User()
            {
                Id = _id,
                Name = _name,
                Surname = _surname, 
                Gender = _gender,
                Birthday = _birthday,
                Avatar = _avatar,
                Education = _education,
                Session = _session,
                AdditionalInfo = _additionalInfo
            };
        }
    }
}
