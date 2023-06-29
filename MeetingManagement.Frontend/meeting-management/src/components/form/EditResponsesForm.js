import React, { useState, useEffect } from 'react';
import 'react-time-picker/dist/TimePicker.css';
import 'react-clock/dist/Clock.css';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const CreateEventForm = () => {
    const [responses, setResponses] = useState([]);

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })
    
    useEffect(() => {
        client.get('/response')
        .then((response) => {
            setResponses(response.data)
        })
        .catch((error) => {
            console.log(error.response.data)
        })
    }, []);

    return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Meetings invitations</b></div>
            <div className="card-body text-start">

            </div>
        </div>
    )
}