import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom'
import { WelcomeMessage } from '../components/ui/WelcomeMessage'
import TimePicker from "react-bootstrap-time-picker";
import { toast } from 'react-toastify';
import axios from 'axios';

export const NoTeam = () => {
    const [accessCode, setAccessCode] = useState('');
    const [createTeam, setCreateTeam] = useState(false);
    const [teamName, setTeamName] = useState('');
    const [startHours, setStartHours] = useState(null);
    const [endHours, setEndHours] = useState(null);

    const styles = {
        main : {
          backgroundColor: "rgb(240,240,240)",
          paddingTop: "120px",
          height: "100vh"
        }
      }

      const apiUrl = process.env.REACT_APP_API_URL
      const client = axios.create({
        withCredentials: true,
        baseURL: apiUrl
      })

    const navigate = useNavigate()

    const handleJoinTeam = async (e) => {
        e.preventDefault()
        var form = document.querySelector('.needs-validation');
        form.classList.add('was-validated');

        if (form.checkValidity())
        {
            await client.post('/team/join?=accessCode' + accessCode)
              .then((response) => {
                console.log(response)
                toast.success("Team joined successfully")
                setTimeout(function() {
                    navigate('/home')
                }, 3000);
              })
              .catch((error) => {
                toast.error(error.response.data)
              });
        }
    }

    const handleCreateTeam = async (e) => {
        e.preventDefault()
        var form = document.querySelector('.needs-validation');
        form.classList.add('was-validated');

        if (form.checkValidity())
        {
            await client.post('/team/create',
            {
                teamName: teamName,
                startWorkingHour: parseInt(new Date(startHours * 1000).toISOString().slice(11, 13)),
                endWorkingHour: parseInt(new Date(endHours * 1000).toISOString().slice(11, 13))
            })
              .then((response) => {
                console.log(response)
                toast.success("Team created successfully")
                setTimeout(function() {
                    navigate('/home')
                }, 3000);
              })
              .catch((error) => {
                toast.error(error.response.data)
              });
        }
    }

    return (
        <div className="container-fluid" style={styles.main}>
          <div className="apx-4 py-5 px-md-5 text-lg-start">
              <div className="row gx-lg-5">
                <WelcomeMessage/>
        
      <div className="col-lg-6 mb-5 mb-lg-0">
        <div className="card">
          <div className="card-body py-5 px-md-5">

            {!createTeam &&
            <form onSubmit={handleJoinTeam} className="needs-validation" noValidate>
            <div className="row container-fluid">
            <label className="form-label" htmlFor="accessCode">Access Code</label>
            <div className="col-sm-8 form-outline mb-3">
                <input value={accessCode} onChange={(e) => setAccessCode(e.target.value)} type="text" id="accessCode" className="form-control" required/>
              </div>
              <button type="submit" className="col-sm-3 btn btn-secondary btn-block mb-3" style={{backgroundColor: "#3474b0"}}>
                Join Team
              </button>
              <span className="col-sm-9 px-3">Don't have a team? <Link onClick={() => setCreateTeam(true)}>Create one</Link></span>
              </div></form>}

              {createTeam &&
              <form onSubmit={handleCreateTeam} className="needs-validation" noValidate>
                <div className="row container-fluid">
                
                <div className="form-outline mb-2">
                    <label className="form-label" htmlFor="teamName">Team Name</label>
                    <input type="text" id="teamName" className="form-control"
                    value={teamName} onChange={(e) => setTeamName(e.target.value)} required/>
                    <div className="invalid-feedback">
                        Please a name for your team.
                    </div>
                </div>

                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="startTime">Start Time</label>
                    <TimePicker id="startTime" className="form-select" step={60} value={startHours} onChange={(start) => setStartHours(start)} required/>
                </div>

                <div className="col-sm-6 form-outline mb-2">
                    <label className="form-label" htmlFor="endTime">End Time</label>
                    <TimePicker id="endTime" className="form-select" step={60} value={endHours} onChange={(end) => setEndHours(end)} required/>
                </div>
                
                <div className="row mx-1 mt-3">
                <button type="submit" className="col-sm-3 btn btn-secondary btn-block mb-3" style={{backgroundColor: "#3474b0"}}>
                    Create Team
                </button>
                <span className="col-sm-9 mt-2 px-3">Already have a team? <Link onClick={() => setCreateTeam(false)}>Join</Link></span>
              </div></div></form>
              }

          </div>
        </div>
      </div>
              </div>
            </div>
        </div>
    )
}