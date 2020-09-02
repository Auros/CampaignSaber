import Link from 'next/link'
import { useStore } from '../utils/useStore'

const authUrl = process.env.NEXT_PUBLIC_API_URL + '/api/authorization'

function NavEnd() {
  const { state, dispatch } = useStore()

  if (state.user) {
    return (
      <>
        <span className="control">
          <div className="buttons">
            <a className="button is-warning">
              {state.user.profile.username}
            </a>
            <a className="button" onClick={() => dispatch({ type: 'clearLoginData', value: null })}>
              Logout
            </a>
          </div>
        </span>
      </>
    )
  }
  else {
    return (
      <p className="control animate__animated animate__fadeIn">
        <a href={authUrl} className="button is-primary">
          Login
        </a>
      </p>
    )
  }
}

export default function Navbar() {
  return (
    <nav className="navbar is-dark">
      <div className="container">
        <div className="navbar-brand">
          <div className="navbar-burger burger">
            <span />
            <span />
            <span />
          </div>
        </div>
        <div className="navbar-menu">
          <div className="navbar-start">
          <Link href="/">
              <a className="navbar-item">
                Home
              </a>
            </Link>
            <Link href="/campaigns">
              <a className="navbar-item">
                Campaigns
              </a>
            </Link>
            <a className="navbar-item">
              Download
            </a>
            <a className="navbar-item">
              FAQ
            </a>
          </div>
          <div className="navbar-end">
            <div className="navbar-item">
              <div className="field is-grouped">
                <NavEnd />
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
  )
}