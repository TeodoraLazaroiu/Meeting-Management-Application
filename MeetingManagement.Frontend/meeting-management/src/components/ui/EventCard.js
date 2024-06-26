import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify';
import axios from 'axios';
import moment from 'moment';

export const EventCard = ({eventDate, eventId}) => {
    const [isLoading, setIsLoading] = useState(true);
    const [event, setEvent] = useState('');
    const [participants, setParticipants] = useState([]);
    const [createdBy, setCreatedBy] = useState('');
    const [response, setResponse] = useState('');
    const [userId, setUserId] = useState('');

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    useEffect(() => {
        setIsLoading(true)
        client.get('/event/' + eventId)
            .then((response) => {
                setEvent(response.data)
                setParticipants(response.data.attendesInfo)
                setCreatedBy(response.data.createdBy)
                setIsLoading(false)
            })
            .catch((error) => {
                console.log(error)
            })

        client.get('/response/' + eventId)
        .then((response) => {
            setResponse(response.data)
        })
        .catch((error) => {
            console.log(error)
            setResponse({
                isAttending: null,
                sendReminder: null,
                reminderTime: null
            })
        })
    }, [eventId]);

    useEffect(() => {
        client.get('/user')
        .then((response) => {
            setUserId(response.data.id)
        })
        .catch((error) => {
            console.log(error)
        })
    }, []);

    const navigate = useNavigate()
    const handleDeleteEvent = () => {
        client.delete('/event?eventId=' + eventId)
        .then(() => {
            toast.success("Event deleted successfully")
                setTimeout(function() {
                    navigate(0)
                }, 2000);
        })
        .catch((error) => {
            console.log(error)
        })
    }

    if (isLoading) return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Meeting details</b></div>
                <div className="card-body">
                    <div className="spinner-border" role="status"></div>
                </div>
        </div>
    )
    else return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Meeting details</b></div>
            <div className="card-body">
                <h5 className="card-title">{event.eventTitle}</h5>
                <div className="text-lg-start mx-4 my-3 card-text">
                <p><b>Attending: </b>
                {response.isAttending === null ? <span>N/A</span> : (response.isAttending === true ? <b className="text-success">YES</b> : <b className="text-danger">NO</b>)}</p>
                <p><b>Description: </b>{event.eventDescription === "" ? "N/A" : event.eventDescription}</p>
                <p><b>Date: </b>{moment(eventDate).format('MMMM Do YYYY')}</p>
                <p><b>Time: </b>{event.startTime} - {event.endTime}</p>
                <p><b>Type: </b>{event.isRecurring ? "Recurring" : "Nonrecurring"}</p>
                <p><b>Participants: </b>{participants.map((attendee) => <li key={attendee.id}>{attendee.firstName} {attendee.lastName}</li>)}</p>
                <p><b>Created by: </b>{createdBy.firstName + " " + createdBy.lastName}</p>
                {userId === createdBy.id && <button onClick={handleDeleteEvent} type="button" className="btn btn-outline-danger">Delete</button>}
                </div>
            </div>
        </div>
    )
}