export const Login = () => {
    return (
        <div class="container-fluid" style={{backgroundColor: "hsl(0, 0%, 96%)", height: "100vh"}}>
          <div class="centered-element px-4 py-5 px-md-5 text-center text-lg-start container">
              <div class="row gx-lg-5 align-items-center">
                <div class="col-lg-6 mb-5 mb-lg-0">
                  <h1 class="my-5 display-3 fw-bold ls-tight">
                    Meeting<br />
                    <span class="text-primary">Management</span>
                  </h1>
                  <p>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Eveniet, itaque accusantium odio, soluta, corrupti aliquam
                    quibusdam tempora at cupiditate quis eum maiores libero
                    veritatis? Dicta facilis sint aliquid ipsum atque?
                  </p>
                </div>
        
                <div class="col-lg-6 mb-5 mb-lg-0">
                  <div class="card">
                    <div class="card-body py-5 px-md-5">
                      <form>
        
                        <div class="form-outline mb-4">
                          <label class="form-label" for="form3Example3">Email address</label>
                          <input type="email" id="form3Example3" class="form-control" />
                        </div>
        
                        <div class="form-outline mb-4">
                          <label class="form-label" for="form3Example4">Password</label>
                          <input type="password" id="form3Example4" class="form-control" />
                        </div>
        
                        <div class="form-check d-flex justify-content-center mb-4">
                          <input class="form-check-input me-2" type="checkbox" value="" id="form2Example33" checked />
                          <label class="form-check-label" for="form2Example33">
                            Subscribe to our newsletter
                          </label>
                        </div>
        
                        <button type="submit" class="btn btn-primary btn-block mb-4">
                          Sign up
                        </button>
        
                        <div class="text-center">
                        </div>
                      </form>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        </div>
    )
}