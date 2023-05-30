import React, { useCallback } from 'react';
import axios from 'axios';
import format from "date-fns/format";
import getDay from "date-fns/getDay";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import { useEffect, useState } from "react";
import { Calendar, dateFnsLocalizer } from "react-big-calendar";
import "react-big-calendar/lib/css/react-big-calendar.css";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

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

const eventsValues = [
    {
        title: 'Daily',
        start: new Date(2023, 4, 15, 10),
        end: new Date(2023, 4, 15, 11),
        allDay: false
    },
    {
        title: 'Meet',
        start: new Date(2023, 4, 20),
        end: new Date(2023, 4, 20),
        allDay: false
    },
    {
        title: 'Demo',
        start: new Date(2023, 4, 7),
        end: new Date(2023, 4, 10)
    }
]

export const CalendarComponent = () => {
    const [jsonEvents, setJsonEvents] = useState([]);
    const [events, setEvents] = useState([]);
    const [date, setDate] = useState(new Date());

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    useEffect((year, month) => {
        const getEvents = async () => {
            console.log('/event?year=' + year + '&month=' + month)
            await client.get('/event?year=' + year + '&month=' + month)
              .then((response) => {
                const data = response.data;
                setJsonEvents(data)
              });
          }
        var year = date.getFullYear()
        console.log(year)
        var month = date.getMonth() + 1
        getEvents(year, month)
    }, [date]);

    useEffect(() => {
        const mapDateAndTime = (time, date) => {
            var timeOnly = time.split(':')
            var dateOnly = date.split('.')
            return new Date(dateOnly[2], dateOnly[1] - 1, dateOnly[0], timeOnly[0], timeOnly[1])
        }
        
        var events = jsonEvents.map(e => ({ title: e.eventTitle, start: mapDateAndTime(e.startTime, e.date), end: mapDateAndTime(e.endTime, e.date)}))
        setEvents(events)

    }, [jsonEvents]);

    const onNavigate = useCallback((newDate) => setDate(newDate), [setDate])

    return (
        <Calendar date={date} onNavigate={onNavigate} localizer={localizer} events={events} startAccessor="start" endAccessor="end" style={{width: 700, height: 500, margin: "50px"}}/>
    )
}