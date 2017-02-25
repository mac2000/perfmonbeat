PerfmonBeat
===========

Simple service to collect and send permon metrics to elasticsearch

For local experiments you may run elasticsearch like so:

```
	docker run -it --rm --name es -p 9200:9200 elasticsearch:alpine
```

To generate list of all metrics you may use following PowerShell snippet:

```
$categories = Get-Counter -ListSet * |? CounterSetName -In 'Processor', 'Memory'
[xml]$doc = New-Object System.Xml.XmlDocument
$counters = $doc.CreateNode('element', 'counters', $null)
foreach($category in $categories | sort CounterSetName) {
    $counters.AppendChild($doc.CreateComment($category.Description)) | Out-Null
    foreach($counter in $category.Counter) {
        $item = $doc.CreateNode('element', 'counter', $null)
        $item.SetAttribute('category', $category.CounterSetName)
        $item.SetAttribute('counter', ($counter -split '\\')[-1])
        if ($category.CounterSetType -eq 'MultiInstance') {
            $item.SetAttribute('instance', '_Total')
        }
        $counters.AppendChild($item) | Out-Null
    }
}
$doc.AppendChild($counters) | Out-Null

$stringWriter = New-Object System.IO.StringWriter
$writer = New-Object System.XMl.XmlTextWriter $stringWriter
$writer.Formatting = 'Indented'
$writer.Indentation = 2
$doc.WriteContentTo($writer)
$writer.Flush()
$stringWriter.Flush()
Write-Output $stringWriter.ToString()
```
