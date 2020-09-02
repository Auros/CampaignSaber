import React from 'react'
import Head from 'next/head'
import Navbar from '../components/navbar'

export default function Home() {
  return (
    <>
      <Head>
        <title>CampaignSaber</title>
      </Head>
      <Navbar />
    </>
  )
}