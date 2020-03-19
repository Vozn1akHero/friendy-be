using System;
using System.Collections.Generic;
using System.Text;
using BE.Models;

namespace BE.FakeData.Builders
{
    public class FakeSessionBuilder
    {
        private int _id = 0;
        private DateTime _connectionStart = DateTime.Now;
        private DateTime? _connectionEnd = null;
        private string _connectionId = new Guid().ToString();

        public FakeSessionBuilder(int id, DateTime connectionStart, string connectionId)
        {
            this._id = id;
            this._connectionStart = connectionStart;
            this._connectionId = connectionId;
        }

        public FakeSessionBuilder WithConnectionEnd(DateTime connectionEnd)
        {
            this._connectionEnd = connectionEnd;
            return this;
        }

        public Session Build()
        {
            return new Session()
            {
                Id = this._id,
                ConnectionStart = _connectionStart,
                    ConnectionEnd = _connectionEnd,
                    ConnectionId = _connectionId
            };
        }
    }
}
