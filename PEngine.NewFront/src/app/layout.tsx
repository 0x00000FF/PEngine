import Link from "next/link";
import "./layout.css";

export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8"></meta>
        <link
          rel="alternate"
          type="application/rss+xml"
          title="RSS Feed"
          href="/Rss"
        />
        <link
          rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css"
        />
      </head>
      <body>
        <header>
          <label className="toggle-menu" htmlFor="asideToggle">
            <i className="fa fa-bars"></i>
          </label>
          <section className="header-search">
            <form className="search-box" asp-controller="Search" method="get">
              <i className="fa-solid fa-magnifying-glass"></i>
              <input type="text" name="keyword" placeholder="Search..." />
            </form>
          </section>
          <menu>
            <a className="login-locked locked">
              <i className="fa fa-lock"></i>
            </a>
          </menu>
        </header>
        <main>
          <input type="checkbox" id="asideToggle" />
          <aside>
            <ul className="gnb">
              <li>
                <Link href="/">Home</Link>
              </li>
              <li>
                <Link href="/search">Search</Link>
              </li>
              <li>
                <Link href="/guestbook">Guestbook</Link>
              </li>
            </ul>
            <div className="aside-mdd"></div>
          </aside>
          <section className="content">{children}</section>
        </main>
        <footer>
          <span>
            <a
              href="https://github.com/0x00000FF/PEngine"
              target="_blank"
              rel="noopener"
            >
              PEngine α NFR
            </a>
            &copy; P.Knowledge, 2024.
          </span>
          <a
            className="mdd"
            href="https://www.conoha.jp/conoha/"
            target="_blank"
            rel="noopener"
          >
            Running on ConoHa
          </a>
        </footer>
      </body>
    </html>
  );
}
