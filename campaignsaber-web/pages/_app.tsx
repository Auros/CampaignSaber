import 'animate.css'
import React from 'react'
import '../styles/styles.scss'
import { AppProps } from 'next/app'
import { Provider } from '../components/store'
import Head from 'next/head'

export default function App({ Component, pageProps }: AppProps) {
  return (
    <Provider>
      <Head>
        <script defer src="https://use.fontawesome.com/releases/v5.3.1/js/all.js"></script>
      </Head>
      <Component {...pageProps} />
    </Provider>
  )
}