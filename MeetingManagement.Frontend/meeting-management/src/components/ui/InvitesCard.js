import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';

export const InvitesCard = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [responses, setResponses] = useState([]);
    const [viewResponses, setViewResponses] = useState(false);
    const [editResponse, setEditResponse] = useState(false);
    const [responseEventTitle, setResponseEventTitle] = useState('');
    const [responseEventId, setResponseEventId] = useState('');
    const [responseIsAttending, setResponseIsAttending] = useState(false);
    const [responseSendReminder, setResponseSendReminder] = useState(false);
    const [responseReminderTime, setResponseReminderTime] = useState(0);

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    useEffect(() => {
        client.get('/response')
            .then((response) => {
                setResponses(response.data)
                setViewResponses(true)
                setEditResponse(false)
                setIsLoading(false)
            })
            .catch((error) => {
                console.log(error)
            })
    }, []);

    const handleEditClick = async (e) => {
        setIsLoading(true)
        setViewResponses(false)
        setEditResponse(true)
        setResponseEventTitle(e.eventTitle)
        setResponseEventId(e.eventId)
        
        await client.get('/response/' + e.eventId)
            .then((response) => {
                setResponseIsAttending(response.data.isAttending)
                setResponseSendReminder(response.data.sendReminder)
                setResponseReminderTime(response.data.reminderTime)
                setIsLoading(false)
            })
            .catch((error) => {
                console.log(error)
            })
    }
    
    const handleCancelEdit = (e) => {
        setViewResponses(true)
        setEditResponse(false)
    }

    const handleSaveEditResponse = async (e) => {
        e.preventDefault()
        console.log(responseIsAttending)
        var data = {
        eventId: responseEventId,
        isAttending: responseIsAttending,
        sendReminder: responseSendReminder,
        reminderTime: parseInt(responseReminderTime)
        }
        console.log(data)
        await client.put('/response', data)
            .then((response) => {
                console.log(response)
                toast.success("Response updated successfully")
                setTimeout(function() {
                    setViewResponses(true)
                    setEditResponse(false)
                }, 3000);
            })
            .catch((error) => {
                console.log(error)
            })
    }

    if (isLoading) return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Meetings invites</b></div>
                <div className="card-body">
                    <div class="spinner-border" role="status"></div>
                </div>
        </div>
    )
    else return (
    <div className="card bg-light" style={{maxWidth: 450}}>
        <div className="card-header"><b>Meeting invites</b></div>

    {viewResponses &&
    <div className="card-body">
        <h5 className="card-title"></h5>
            <div className="text-lg-start mx-4 card-text">
            <table className="table">
            <thead>
                <tr>
                <th scope="col">Event</th>
                <th scope="col">Going</th>
                <th scope="col">Reminder</th>
                <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                {responses.length !== 0 && responses.map((e) => <tr key={e.eventId}>
                <td>{e.eventTitle}</td>
                <td>{e.isAttending === null ? <span>N/A</span> : (e.isAttending === true ?
                <b className="text-success">YES</b> : <b className="text-danger">NO</b>)}</td>

                <td>{e.sendReminder === null ? <span>N/A</span> : (e.sendReminder === false ?
                <b className="text-danger">NO</b> : <b className="text-success">YES, {e.reminderTime}m</b>)}</td>

                <td><button onClick={() => handleEditClick(e)} type="button" className="btn btn-secondary btn-block" style={{backgroundColor: "#3474b0"}}>
                    Edit</button></td>
                </tr>)}
            </tbody>
        </table>
            </div>
        </div>}

    {editResponse &&
    <div className="card-body">
        <h5 className="card-title mx-4 text-lg-start">Meeting: {responseEventTitle}</h5>
            <div className="text-lg-start mx-4 my-3 card-text">
                <form onSubmit={handleSaveEditResponse}>
                    <div className="col-sm-6 form-outline mb-2">
                        <label className="mb-2" htmlFor="isAttending">Going</label>
                        <select className="form-select" id="isAttending" onChange={(e) => setResponseIsAttending(e.target.value)} value={responseIsAttending === null ? '' : responseIsAttending}>
                            <option value={true}>YES</option>
                            <option value={false}>NO</option>
                            <option disabled value={''}>N/A</option>
                        </select>
                    </div>
                    <div className="col-sm-6 form-outline mb-2">
                        <label className="mb-2" htmlFor="sendReminder">Send reminder</label>
                        <select className="form-select" id="sendReminder" onChange={(e) => setResponseSendReminder(e.target.value)} value={responseSendReminder === null ? '' : responseSendReminder}>
                            <option value={true}>YES</option>
                            <option value={false}>NO</option>
                            <option disabled value={''}>N/A</option>
                        </select>
                    </div>
                    <div className="col-sm-6 form-outline mb-2">
                        <label className="mb-2" htmlFor="reminderTime">Reminder time</label>
                        <select className="form-select" id="reminderTime" onChange={(e) => setResponseReminderTime(e.target.value)} value={responseReminderTime === null ? 0 : responseReminderTime}>
                            <option value={5}>5</option>
                            <option value={10}>10</option>
                            <option value={15}>15</option>
                            <option value={20}>20</option>
                            <option value={25}>25</option>
                            <option value={30}>30</option>
                            <option disabled value={''}>N/A</option>
                        </select>
                    </div>
                    <div className="my-3 row container-fluid">
                        <button onClick={handleCancelEdit} type="button" className="mx-2 col-sm-3 btn btn-secondary btn-block">
                            Cancel
                        </button>
                        <button type="submit" className="col-sm-3 btn btn-secondary btn-block" style={{backgroundColor: "#3474b0"}}>
                            Save
                        </button>
                    </div>
                </form>
            </div>
        </div>}
    </div>
    )
}