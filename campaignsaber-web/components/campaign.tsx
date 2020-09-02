import Link from 'next/link'

export default function Campaign() {
  return (
    <div className="box">
      <article className="media">
        <figure className="media-left">
          <p className="image is-192x192">
            <img src="https://localhost:44321/files/bb8fd1ef-c715-43e6-8510-82f8399d5477/8b3c37c1ba940cdc717947a130de39ed1d5735969f1d622a8a483f20d1f5e4cc.png"/>
          </p>
        </figure>
        <div className="media-content">
          <Link href="/">
            <h3 className="title"><a>Bloq Busters (Level 1)</a></h3>
          </Link>
          <h3 className="subtitle">uploaded by <Link href="/"><a>Uploader</a></Link></h3>
        </div>
        <div className="media-right">
          <div className="content has-text-right">
            <button className="button is-link">Download</button>
          </div>
        </div>
      </article>
    </div>
  )
}