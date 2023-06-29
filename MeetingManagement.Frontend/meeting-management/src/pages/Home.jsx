import { Navbar } from '../components/ui/Navbar';
import React, { useCallback, useContext } from 'react';
import axios from 'axios';
import format from "date-fns/format";
import getDay from "date-fns/getDay";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import { useEffect, useState } from "react";
import { Calendar, dateFnsLocalizer } from "react-big-calendar";
import AuthContext from '../utils/AuthContext';
import "react-big-calendar/lib/css/react-big-calendar.css";
import "react-datepicker/dist/react-datepicker.css";
import { EventCard } from '../components/ui/EventCard';
import { InvitesCard } from '../components/ui/InvitesCard';
import { CreateEventForm } from '../components/form/CreateEventForm';

const locales = {
    "en-GB": require('date-fns/locale/en-GB')
}

const localizer = dateFnsLocalizer({
    format,
    parse,
    startOfWeek,
    getDay,
    locales
})

export const Home = () => {
    const [jsonEvents, setJsonEvents] = useState([]);
    const [events, setEvents] = useState([]);
    const [date, setDate] = useState(new Date());
    const [selectedEvent, setSelectedEvent] = useState(false);
    const [selectedEventDate, setSelectedEventDate] = useState('');
    const [eventId, setEventId] = useState('');
    const [createNewEvent, setCreateNewEvent] = useState(false);
    const [viewInvites, setViewInvites] = useState(true);

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
        withCredentials: true,
        baseURL: apiUrl
    })

    const { setAuthenticated } = useContext(AuthContext);

    client.interceptors.response.use(function (response) {
        return response;
    }, function (error) {
        if (!error.response) {
            setAuthenticated(false)
        }
    });

    useEffect((year, month) => {
        const getEvents = async () => {
        await client.get('/event?year=' + year + '&month=' + month)
            .then((response) => {
                const data = response.data;
                setJsonEvents(data)
            })
            .catch((error) => {
                console.log(error)
            })
            }
        var year = date.getFullYear()
        var month = date.getMonth() + 1
        getEvents(year, month)
    }, [date]);

    useEffect(() => {
        const mapDateAndTime = (time, date) => {
            var timeOnly = time.split(':')
            var dateOnly = date.split('/')
            return new Date(dateOnly[2], dateOnly[0] - 1, dateOnly[1], timeOnly[0], timeOnly[1])
        }
        
        var events = jsonEvents.map(e => ({ id: e.id, title: e.eventTitle, start: mapDateAndTime(e.startTime, e.date), end: mapDateAndTime(e.endTime, e.date)}))
        setEvents(events)

    }, [jsonEvents]);

    const onSelectEvent = useCallback((callEvent) => {
        setCreateNewEvent(false)
        setViewInvites(false)
        setSelectedEvent(true)
        setEventId(callEvent.id)
        setSelectedEventDate(callEvent.start)
      }, [])

    const handleCreateNewEvent = (e) => {
        e.preventDefault()
        setSelectedEvent(false)
        setViewInvites(false)
        setCreateNewEvent(true)
    }

    const handleViewInvites = (e) => {
        e.preventDefault()
        setViewInvites(true)
        setSelectedEvent(false)
        setCreateNewEvent(false)
    }

    const onNavigate = useCallback((newDate) => setDate(newDate), [setDate])

    return (
        <div>
            <Navbar/>
        <div className="container-fluid">
        <div className="row">
          <div className="col">
            <div className="mx-5" style={{marginTop: 25}}>
                <h3 style={{marginBottom: 25, float: "left"}}><b>Your calendar</b></h3>
                <button onClick={handleCreateNewEvent} className="mt-2 mx-1 btn btn-secondary btn-block" style={{backgroundColor: "#3474b0", float: "right"}}>Create new event</button>
                <button onClick={handleViewInvites} className="mt-2 mx-1 btn btn-secondary btn-block" style={{float: "right"}}>Invites</button>
            </div>
          <Calendar date={date} onNavigate={onNavigate} localizer={localizer} events={events} onSelectEvent={onSelectEvent} style={{width: 700, height: 500, margin: "50px"}} popup/>
          </div>
          <div className="col mx-2 my-5">
          {selectedEvent && <EventCard eventDate={selectedEventDate} eventId={eventId}/>}
          {viewInvites && <InvitesCard/>}
          {createNewEvent && <CreateEventForm/>}
          </div>
        </div>
        </div>
        </div>
    )
}