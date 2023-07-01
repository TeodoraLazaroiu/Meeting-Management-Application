import React, { useEffect, useState } from 'react';
import axios from 'axios';

export const TeamInfoCard = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [teamInfo, setTeamInfo] = useState('');

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    useEffect(() => {
        setIsLoading(true)
        client.get('/team')
        .then((response) => {
            setTeamInfo(response.data)
            setIsLoading(false)
        })
        .catch((error) => {
            console.log(error)
        })
    }, []);

    if (isLoading) return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Team Info</b></div>
                <div className="card-body">
                    <div className="spinner-border" role="status"></div>
                </div>
        </div>
    )
    else return (
        <div className="card bg-light" style={{maxWidth: 450}}>
            <div className="card-header"><b>Team Info</b></div>
            <div className="card-body">
                <div className="text-lg-start mx-4 my-3 card-text">
                <p><b>Team name: </b>{teamInfo.teamName}</p>
                <p><b>Access code: </b>{teamInfo.accessCode}</p>
                <p><b>Working hours: </b>{teamInfo.startWorkingHour}:00 - {teamInfo.endWorkingHour}:00</p>
                </div>
            </div>
        </div>
    )
}