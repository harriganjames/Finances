using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Engines;
using System.Collections;
using System.Collections.Generic;

namespace Finances.UnitTests.MS.Core.Engines.ScheduleTests
{
    [TestClass]
    public class ScheduleTests
    {
        //IScheduleInfo sut;
        IEnumerable<IScheduleFrequencyCalculator> scheduleFrequencyCalculators; 

        [TestInitialize]
        public void Initialize()
        {

            //sut = new ScheduleInfo();
            scheduleFrequencyCalculators = new IScheduleFrequencyCalculator[] { 
                                new ScheduleFrequencyCalculatorMonthly(),
                                new ScheduleFrequencyCalculatorWeekly() };

        }


        [TestMethod]
        public void TestGetDescription()
        {

            var tests = new[] 
            { 
                new 
                {   
                    TestName = "One-off on 2015-10-10",
                    Schedule = new Schedule(scheduleFrequencyCalculators)
                    {
                        StartDate = new DateTime(2015,10,10),
                        EndDate = new DateTime(2015,10,10)
                    }
                },
                new 
                {   
                    TestName = "Every 1 month from 2015-10-10",
                    Schedule = new Schedule(scheduleFrequencyCalculators)
                    {
                        StartDate = new DateTime(2015,10,10),
                        Frequency = "Monthly",
                        FrequencyEvery = 1
                    }
                },
                new 
                {   
                    TestName = "Every 2 months from 2015-10-10",
                    Schedule = new Schedule(scheduleFrequencyCalculators)
                    {
                        StartDate = new DateTime(2015,10,10),
                        Frequency = "Monthly",
                        FrequencyEvery = 2
                    }
                },
                new 
                {   
                    TestName = "Every 2 months from 2015-10-10 until 2016-10-10",
                    Schedule = new Schedule(scheduleFrequencyCalculators)
                    {
                        StartDate = new DateTime(2015,10,10),
                        EndDate = new DateTime(2016,10,10),
                        Frequency = "Monthly",
                        FrequencyEvery = 2
                    }
                },

            };


            foreach (var test in tests)
            {
                var d = test.Schedule.GetDescription();

                Assert.AreEqual(test.TestName, d, test.TestName);

            }


        }
    }
}
