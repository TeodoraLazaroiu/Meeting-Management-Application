import React, { useState, useEffect } from 'react';
import DatePicker from 'react-datepicker';
import 'react-time-picker/dist/TimePicker.css';
import 'react-clock/dist/Clock.css';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import TimePicker from "react-bootstrap-time-picker";
import Select from 'react-select';

export const CreateEventForm = () => {
    const [eventTitle, setEventTitle] = useState('');
    const [eventDescription, setEventDescription] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');
    const [isRecurring, setIsRecurring] = useState(false);
    const [recurrenceType, setRecurrenceType] = useState(0);
    const [separation, setSeparation] = useState(false);
    const [separationCount, setSeparationCount] = useState(0);
    const [recurrenceUnit, setRecurrenceUnit] = useState('');
    const [selectedDaysOfWeek, setSelectedDaysOfWeek] = useState([]);
    const [formNumber, setFormNumber] = useState(1);
    const [teamUsers, setTeamUsers] = useState([]);
    const [selectedUsers, setSelectedUsers] = useState([]);
    const [currentUserId, setCurrentUserId] = useState('');
    const [suggestedIntervals, setSuggestedIntervals] = useState([]);
    const [clickedSuggestions, setClickedSuggestions] = useState(false);

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    const postEvent = async (eventJson) => {
        await client.post('/event', eventJson)
        .then((response) => {
            console.log(response)
            if (response.status === 400)
            {
                toast.error(response)
            }
            else toast.success("Event created successfully")
        })
        .catch((error) => {
            console.log(error)
            toast.error(error.response.data)
        })
          }

    const handleCreateEvent = async (e) => {
        e.preventDefault()
        var event = {
            eventTitle: eventTitle,
            eventDescription: eventDescription,
            attendes: selectedUsers.map(e => e.value),
            startTime: new Date(startTime * 1000).toISOString().slice(11, 16),
            endTime: new Date(endTime * 1000).toISOString().slice(11, 16),
            isRecurring: isRecurring,
            startDate: new Date(startDate).toLocaleDateString('en-GB'),
            endDate: new Date(endDate).toLocaleDateString('en-GB'),
            recurrenceType: parseInt(recurrenceType),
            separationCount: separationCount,
            daysOfWeek: [],
            dayOfWeek: 0,
            dayOfMonth: 0
        }
        event.attendes.push(currentUserId);

        if (recurrenceType === '1') {
            event.daysOfWeek = selectedDaysOfWeek
        }
        else if (recurrenceType === '2')
        {
            var day = new Date(startDate).getDay()
            if (day === 0) day = 7;
            event.dayOfWeek = day
        }
        else if (recurrenceType === '3')
        {
            event.dayOfMonth = new Date(startDate * 1000).getDate()
        }
        console.log(event)
        postEvent(event)
    }

    const handleRecurringCheckbox = () => {
        if (isRecurring) 
        {
            setRecurrenceType('')
            setSeparation(false)
        }
        setIsRecurring(!isRecurring)
    }

    const handleRecurrenceType = (e) => {
        setRecurrenceType(e.target.value)
        if (e.target.value === "1") setRecurrenceUnit('day')
        else if (e.target.value === "2") setRecurrenceUnit('week')
        else if (e.target.value === "3") setRecurrenceUnit('month')
        else setRecurrenceType('')
    }

    const handleDaysOfWeek = (e) => {
        let days = Array.from(e.target.selectedOptions, option => parseInt(option.value));
        setSelectedDaysOfWeek(days)
      }

    useEffect(() => {
        if (formNumber === 2)
        {
            var userId;
            client.get('/user')
            .then((response) => {
                userId = response.data.id
                setCurrentUserId(userId)
            })
            .catch((error) => {
                console.log(error.response.data)
            })

            client.get('/team')
            .then((response) => {
                var allUsers = response.data.teamMembers
                var users = allUsers.filter(u => u.id !== userId)
                setTeamUsers(users)
            })
            .catch((error) => {
                toast.error(error.response.data)
            })
        }
    }, [formNumber]);

    useEffect(() => {
        if (clickedSuggestions === true)
        {
            var data = {
                attendes: selectedUsers.map(e => e.value),
                date: new Date(startDate).toLocaleDateString('en-GB')
            }
            data.attendes.push(currentUserId);

            console.log(data)
            client.post('/event/intervals', data)
            .then((response) => {
                console.log(response)
                setSuggestedIntervals(response.data)
            })
            .catch((error) => {
                console.log(error.response.data)
            })
            setClickedSuggestions(false)
        }
    }, [clickedSuggestions]);

    useEffect(() => {
        if (startDate !== '' && endDate === '')
        {
            setEndDate(startDate)
        }
    }, [startDate]);

    if (formNumber === 1) return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Create new event</b></div>
            <div className="card-body text-start">
            <form autoComplete="off" className="needs-validation" noValidate>
              <div className="form-outline mb-2">
                <label className="form-label" htmlFor="eventTitle">Event Title</label>
                <input type="text" id="eventTitle" className="form-control"
                value={eventTitle} onChange={(e) => setEventTitle(e.target.value)} required/>
                <div className="invalid-feedback">
                    Please a title for your event.
                </div>
              </div>
              
              <div className="form-outline mb-2">
                <label className="form-label" htmlFor="eventDescription">Event Description</label>
                <input type="text" id="eventDescription" className="form-control"
                value={eventDescription} onChange={(e) => setEventDescription(e.target.value)}/>
              </div>

            <div className="row">
                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="startDate">Start Date</label>
                    <DatePicker dateFormat="dd-MM-yyyy" id="startDate" className="form-control" value={startDate} selected={startDate} onChange={(start) => setStartDate(start)} required/>
                </div>

                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="endDate">End Date</label>
                    <DatePicker dateFormat="dd-MM-yyyy" id="endDate" className="form-control" value={endDate} selected={endDate} onChange={(end) => setEndDate(end)} required/>
                </div>
            </div>

            <div className="row container">
                <div className="col-sm-6 my-3 form-check">
                    <label className="form-check-label" htmlFor="isRecurring">Recurring Meeting</label>
                    <input type="checkbox" className="form-check-input" id="isRecurring" value={isRecurring} onClick={handleRecurringCheckbox}/>
                </div>
                
                {recurrenceType !== 0 &&
                <div className="col-sm-6 my-3 form-check">
                    <label className="form-check-label" htmlFor="separation">Repeat every {recurrenceUnit}</label>
                    <input type="checkbox" className="form-check-input" id="separation" value={!separation} onClick={() => setSeparation(!separation)} checked={!separation}/>
                </div>
                }
            </div>

            <div className="row mb-2">
                {isRecurring &&
                        <div className="col-sm-6 form-outline mb-2">
                            <label className="mb-2" htmlFor="recurrenceType">Recurrence Type</label>
                            <select className="form-select" id="recurrenceType" onChange={handleRecurrenceType}>
                                <option disabled selected> -- select option -- </option>
                                <option value="1">Daily</option>
                                <option value="2">Weekly</option>
                                <option value="3">Monthly</option>
                            </select>
                        </div>}

                {(separation && recurrenceType !== '' && recurrenceType !== '1') &&
                    <div className="col-sm-6 form-outline mb-2">
                        <label className="mb-2" htmlFor="separationCount">Repeat every ..</label>
                        <select className="form-select" id="separationCount" onChange={(e) => setSeparationCount(parseInt(e.target.value) - 1)}>
                            <option disabled selected value> -- select option -- </option>
                            <option value="2">2 {recurrenceUnit}s</option>
                            <option value="3">3 {recurrenceUnit}s</option>
                            <option value="4">4 {recurrenceUnit}s</option>
                            <option value="5">5 {recurrenceUnit}s</option>
                            <option value="6">6 {recurrenceUnit}s</option>
                        </select>
                    </div>
                }
            </div>

            {(separation && recurrenceType === "1") &&
                <div className="form-outline mb-2" style={{height: 150}}>
                    <label htmlFor="daysOfWeek">Days of week</label>
                    <select multiple onChange={handleDaysOfWeek} className="form-select" id="daysOfWeek">
                        <option value="1">Monday</option>
                        <option value="2">Tuesday</option>
                        <option value="3">Wednesday</option>
                        <option value="4">Thursday</option>
                        <option value="5">Friday</option>
                        <option value="6">Saturday</option>
                        <option value="7">Sunday</option>
                    </select>
                </div>}

              <div className="row container-fluid">
              <button type="button" className="mx-2 col-sm-3 btn btn-secondary btn-block" style={{backgroundColor: "#3474b0"}} onClick={(e) => setFormNumber(2)}>
                Next
              </button>
              </div>
            </form>
            </div>
        </div>
    )

    if (formNumber === 2) return (
        <div onSubmit={handleCreateEvent} className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Create new event</b></div>
            <div className="card-body text-start">
            <form autoComplete="off" className="needs-validation" noValidate>

            <div className="row">
                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="startDate">Start Date</label>
                    <DatePicker dateFormat="dd-MM-yyyy" id="startDate" className="form-control" value={startDate} selected={startDate} onChange={(start) => setStartDate(start)} required/>
                </div>

                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="endDate">End Date</label>
                    <DatePicker dateFormat="dd-MM-yyyy" id="endDate" className="form-control" value={endDate} selected={endDate} onChange={(end) => setEndDate(end)} required/>
                </div>
            </div>

            <div className="row">
                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="startTime">Start Time</label>
                    <TimePicker initialValue="00:00" format={24} id="startTime" start="7:00" end="20:00" className="form-select" value={startTime} onChange={(start) => setStartTime(start)} required/>
                </div>

                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="endTime">End Time</label>
                    <TimePicker initialValue="00:00" format={24} id="endTime" start="7:00" end="20:00" className="form-select" value={endTime} onChange={(end) => setEndTime(end)} required/>
                </div>
            </div>

            <div className="row">
                <div className="form-outline my-2">
                    <label className="form-label" htmlFor="users">Add participants</label>
                    <Select isMulti id="users" options={teamUsers.map(e => ({ value: e.id, label: e.firstName + " " + e.lastName}))} name="users" className="basic-multi-select" classNamePrefix="select"
                    onChange={(e) => setSelectedUsers(e)} required/>
                </div>
            </div>

            <div className="my-3 row container-fluid">
              <button onClick={() => setFormNumber(1)} type="button" className="mx-2 col-sm-3 btn btn-secondary btn-block">
                Back
              </button>
              <button type="submit" className="col-sm-3 btn btn-secondary btn-block" style={{backgroundColor: "#3474b0"}}>
                Create
              </button>
              </div>

              <div className="my-3 row container-fluid">
              <button onClick={() => setClickedSuggestions(true)} type="button" className="mx-2 col-sm-6 btn btn-outline-dark btn-block">
                Get suggestions {suggestedIntervals.length !== 0 && <i class="bi bi-arrow-counterclockwise"></i>}</button>
              </div>

              <div className="my-3 row container-fluid">
              {suggestedIntervals.length !== 0 && suggestedIntervals.map(i => <div class="border border-dark text-center rounded mx-2 col-sm-3">{i.startTime} - {i.endTime} <i class="bi bi-clock"></i></div>)}
              </div>

            </form>
            </div>
        </div>
    )
}