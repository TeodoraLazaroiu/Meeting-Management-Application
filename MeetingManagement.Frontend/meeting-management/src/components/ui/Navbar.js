import React from 'react';
import { Link } from 'react-router-dom'

export const Navbar = () => {
    return (
    <nav class="navbar navbar-expand-lg navbar-dark" style={{backgroundColor: "#3474b0"}}>
        <Link class="text-lg-start navbar-brand" to="/home">
        <h5 className="mx-4 display-7 fw-bold">
            Meeting <br />
            <span className="text-black">Management</span>
            </h5>
        </Link>
        <div class="navbar-nav">
            <Link class="nav-item nav-link active" to="/home">Home</Link>
            <Link class="nav-item nav-link" to="">Features</Link>
            <Link class="nav-item nav-link" to="/login">Logout</Link>
        </div>
    </nav>
    )
}