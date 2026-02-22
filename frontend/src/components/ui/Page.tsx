export function Page(props: { title: string; subtitle?: string; children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100">
      <div className="mx-auto max-w-6xl px-8 py-14">
        <header className="mb-12">
          <h1 className="text-3xl font-semibold tracking-tight text-slate-900">
            {props.title}
          </h1>

          {props.subtitle && (
            <p className="mt-3 text-sm text-slate-500">
              {props.subtitle}
            </p>
          )}
        </header>

        {props.children}
      </div>
    </div>
  );
}