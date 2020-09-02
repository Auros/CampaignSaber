import React from 'react'
import Head from 'next/head'
import Navbar from '../../components/navbar'
import Campaign from '../../components/campaign'
import InfiniteScroll from 'react-infinite-scroll-component'

export default function Campaigns() {
  return (
    <>
      <Head>
        <title>Campaigns</title>
      </Head>
      <Navbar />
      <section className="section">
        <div className="container">
          <InfiniteScroll
            dataLength={5}
            next={() => console.log('next') }
            hasMore={true}
            loader={'eh'}
            >
            
            
          </InfiniteScroll>
        </div>
      </section>
    </>
  )
}