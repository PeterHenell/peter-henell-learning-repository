from django.conf.urls import patterns, include, url

from django.contrib import admin
admin.autodiscover()

from django.shortcuts import render

def my_view(request):
    # View code here...
    return render(request, 'doc.jade', {"foo": "bar"})

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'pg_dgen.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),

    url(r'^admin/', include(admin.site.urls)),
    url(r'^home', my_view, name='home'),
    url(r'^$', my_view, name='home'),
)
