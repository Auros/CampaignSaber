import useSWR from 'swr'
import React from 'react'
import fetch from 'unfetch'
import Router, { useRouter } from 'next/router'
import { useStore } from '../../utils/useStore'

const authUrl = process.env.NEXT_PUBLIC_API_URL
const delay = (ms: number) => new Promise(res => setTimeout(res, ms));
const fetcher = (url: RequestInfo) => fetch(url).then(r => r.json())
const authFetcher = (url: RequestInfo, token: string) => fetch(url, { headers: { 'Authorization': `Bearer ${token}` } }).then(r => r.json())

function getToken(code: string) {
  const { data, error } = useSWR(`${authUrl}/api/authorization/callback?code=${code}`, fetcher)
  return {
    response: data,
    isLoading: !data && !error,
    isError: error
  }
}

function getProfile(token: string) {
  const { data, error } = useSWR([authUrl + '/api/authorization/@me', token], authFetcher)
  return {
    user: data,
    isLoading: !data && !error,
    isError: error
  }
}

function Loader({ code, dispatcher }) {
  const { response, isLoading, isError } = getToken(code)
  if (isLoading) {
    return <Authorizing text="Authorizing..." color="" />
  }
  if (response) {
    return <Profiler token={response.token} dispatcher={dispatcher} />
  }
  if (isError) {
    goBack(dispatcher, null)
    return <Error />
  }
}

function Profiler({ token, dispatcher }) {
  const { user, isLoading, isError } = getProfile(token)
  if (isLoading) {
    return <Authorizing text="Loading Profile" color="is-warning" />
  }
  if (user) {
    goBack(dispatcher, { token, user, expiration: Math.round((new Date()).getTime() / 1000) + 1209600 })
    return <Authorizing text="Redirecting..." color="is-success" doLoad={false}  />
  }
  if (isError) {
    goBack(dispatcher, null)
    return <Error />
  }
}


async function goBack(storeDispatch: any, stateObject: any) {
  await delay(1500)
  storeDispatch({ type: stateObject ? 'setLoginData' : 'clearLoginData', value: stateObject })
  Router.push('/')
}

function Authorizing({ text,  color, doLoad = true }) {
  return (
    <div className="box is-inline-block has-text-centered animate__animated animate__fadeIn">
      <div className="content animate__animated animate__fadeIn">
        <h1 className="title">{text}</h1>
        <progress className={`progress is-large ${color ?? ""}`} max="100" value={doLoad ? undefined : 100} >0%</progress>
      </div>
    </div>
  )
}

function Error() {
 return (
  <div className="box is-inline-block has-text-centered animate__animated animate__fadeIn">
    <div className="content animate__animated animate__headShake">
      <h1 className="title has-text-danger">Uh Oh!</h1>
      <h5 className="subtitle">An error has occured</h5>
      <p className="subtitle">Please try again later</p>
    </div>
  </div>
 )
} 

export default function Login() {
  const { dispatch }= useStore()
  const code: string = useRouter()?.query?.code as string
  if (code) {
    return (
      <>
        <div className="hero is-fullheight">
          <div className="hero-body">
            <div className="container">
              <div className="columns is-mobile is-centered">
                <div className="column is-half has-text-centered">
                  <Loader code={code} dispatcher={dispatch} />
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    )
  }
  else {
    return (
      <>
        <div className="hero is-fullheight">
          <div className="hero-body">
            <div className="container">
              <div className="columns is-mobile is-centered">
                <div className="column is-half has-text-centered">
                  <Authorizing text="Authorizing..." color="is-warning" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    )
  }
}