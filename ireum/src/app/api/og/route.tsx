import { ImageResponse } from 'next/og';
import { NextRequest } from 'next/server';

export const runtime = 'edge';

// Safari 3.x UA → Google Fonts returns TTF/truetype format.
// Satori only supports OTF/TTF, not woff or woff2.
// IE UA returns EOT, Chrome 36+ returns woff2 — neither works with resvg.
const TTF_UA =
  'Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-us) AppleWebKit/525.28.3 (KHTML, like Gecko) Version/3.2.3 Safari/525.29';

async function loadFontsForText(text: string): Promise<ArrayBuffer[]> {
  const css = await fetch(
    `https://fonts.googleapis.com/css?family=Noto+Sans+KR:900&text=${encodeURIComponent(text)}`,
    { headers: { 'User-Agent': TTF_UA } }
  ).then(r => {
    if (!r.ok) throw new Error(`Google Fonts CSS ${r.status}`);
    return r.text();
  });

  const urls = [
    ...css.matchAll(/url\(['"]?(https?:\/\/fonts\.gstatic\.com\/[^'")\s]+)['"]?\)/g),
  ].map(m => m[1]);

  if (!urls.length) throw new Error('No font URLs in CSS');

  return Promise.all(
    urls.map(u => fetch(u).then(r => {
      if (!r.ok) throw new Error(`Font fetch ${r.status}`);
      return r.arrayBuffer();
    }))
  );
}

export async function GET(req: NextRequest) {
  const { searchParams } = new URL(req.url);

  // Data passed as URL params from generateMetadata — no DB query needed
  const nameHangul = searchParams.get('n') ?? '이름';
  const nameRoman = (searchParams.get('r') ?? 'IREUM').toUpperCase();
  const inputName = searchParams.get('f'); // "for: <name>"
  const mode = searchParams.get('m') ?? 'my_name';
  // h = "char·meaning|char·meaning" (pipe-separated pairs)
  const hanjaRaw = searchParams.get('h') ?? '';
  const topHanja = hanjaRaw
    ? hanjaRaw.split('|').slice(0, 2).map(part => {
        const [char, meaning] = part.split('·');
        return { char: char ?? '', meaning: meaning ?? '' };
      })
    : [];

  const allChars = [
    ...new Set([
      ...nameHangul,
      ...nameRoman,
      ...(inputName ?? '').toUpperCase(),
      ...'KOREANAMEFORBABYGTgtyouraen ·↗이름GENERATOR',
    ]),
  ].join('');

  const fontBuffers = await loadFontsForText(allChars);

  const charCount = nameHangul.length;
  const kFontSize = charCount <= 2 ? 160 : charCount === 3 ? 120 : 96;

  const labelText = inputName
    ? mode === 'baby_name'
      ? `BABY NAME FOR ${inputName.toUpperCase()}`
      : `KOREAN NAME FOR ${inputName.toUpperCase()}`
    : 'KOREAN NAME GENERATOR';

  return new ImageResponse(
    (
      <div
        style={{
          width: 1200,
          height: 630,
          backgroundColor: '#FAF8F4',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          position: 'relative',
          overflow: 'hidden',
          fontFamily: 'NotoSansKR',
        }}
      >
        {/* Pink glow — top right */}
        <div
          style={{
            position: 'absolute',
            top: -80,
            right: -80,
            width: 520,
            height: 520,
            borderRadius: '50%',
            backgroundImage:
              'radial-gradient(circle, rgba(220,80,130,0.10) 0%, rgba(220,80,130,0) 65%)',
          }}
        />
        {/* Purple glow — bottom left */}
        <div
          style={{
            position: 'absolute',
            bottom: -60,
            left: -60,
            width: 400,
            height: 400,
            borderRadius: '50%',
            backgroundImage:
              'radial-gradient(circle, rgba(147,51,234,0.08) 0%, rgba(147,51,234,0) 65%)',
          }}
        />

        {/* Logo */}
        <div
          style={{
            position: 'absolute',
            top: 48,
            left: 56,
            display: 'flex',
            alignItems: 'baseline',
            gap: 10,
          }}
        >
          <span style={{ fontSize: 28, fontWeight: 900, color: '#FF2D6B' }}>
            Ireum
          </span>
          <span style={{ fontSize: 14, color: 'rgba(26,10,46,0.28)' }}>
            이름
          </span>
        </div>

        {/* Center content */}
        <div
          style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}
        >
          <div
            style={{
              fontSize: 17,
              color: 'rgba(26,10,46,0.32)',
              letterSpacing: '0.13em',
              marginBottom: 28,
            }}
          >
            {labelText}
          </div>

          <div
            style={{
              fontSize: kFontSize,
              fontWeight: 900,
              color: '#1A0A2E',
              letterSpacing: '-0.02em',
              lineHeight: 1,
            }}
          >
            {nameHangul}
          </div>

          <div
            style={{
              fontSize: 26,
              color: 'rgba(26,10,46,0.36)',
              letterSpacing: '0.20em',
              marginTop: 20,
            }}
          >
            {nameRoman}
          </div>

          {topHanja.length > 0 && (
            <div style={{ display: 'flex', gap: 12, marginTop: 30 }}>
              {topHanja.map((h, i) => (
                <div
                  key={i}
                  style={{
                    backgroundColor: 'rgba(255,45,107,0.07)',
                    border: '1px solid rgba(255,45,107,0.18)',
                    borderRadius: 99,
                    padding: '9px 22px',
                    fontSize: 18,
                    color: '#CC2255',
                  }}
                >
                  {`${h.char} · ${h.meaning}`}
                </div>
              ))}
            </div>
          )}
        </div>

        {/* CTA — bottom center */}
        <div
          style={{
            position: 'absolute',
            bottom: 44,
            fontSize: 16,
            color: 'rgba(26,10,46,0.20)',
            letterSpacing: '0.05em',
          }}
        >
          Get your Korean name ↗
        </div>
      </div>
    ),
    {
      width: 1200,
      height: 630,
      fonts: fontBuffers.map(data => ({
        name: 'NotoSansKR',
        data,
        weight: 900 as const,
        style: 'normal' as const,
      })),
    }
  );
}
